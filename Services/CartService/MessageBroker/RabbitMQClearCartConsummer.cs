using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Cart.API.Domain.Dtos.Common;
using System.Text;
using Newtonsoft.Json;
using Cart.API.Manager;

namespace Cart.API.MessageBroker
{
    public class RabbitMQClearCartConsummer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqSettings _rabbitMQettings;

        public RabbitMQClearCartConsummer(IServiceProvider serviceProvider, IOptions<RabbitMqSettings> settings)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQettings = settings.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQettings.HostName,
                Port     = _rabbitMQettings.Port,
                UserName = _rabbitMQettings.UserName,
                Password = _rabbitMQettings.Password,
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var exchange = _rabbitMQettings.OrderExchangeName;
            var queue    = _rabbitMQettings.ClearCartQueueName;
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true);
            await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queue, exchange, queue);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                try
                {
                    var sessionEvent = JsonConvert.DeserializeObject<string>(message);
                    if (sessionEvent != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var resisService = scope.ServiceProvider.GetRequiredService<IRedisCartService>();
                        await resisService.RemoveCartAsync(sessionEvent);

                        // Manually acknowledge only after successful processing
                        await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process message: {ex.Message}");
                    // Optional: Nack and requeue or send to a dead-letter queue
                    await channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(queue: queue, autoAck: false, consumer: consumer);
        }
    }
}
