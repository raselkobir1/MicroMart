using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notification.API.Domain.Dto.Common;
using Notification.API.Manager.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Notification.API.MessageBroker
{
    public class RabbitMQConsummer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqSettings _rabbitMQettings;

        public RabbitMQConsummer(IServiceProvider serviceProvider, IOptions<RabbitMqSettings> settings)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQettings = settings.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQettings.HostName,
                Port = _rabbitMQettings.Port,
                UserName = _rabbitMQettings.UserName,
                Password = _rabbitMQettings.Password,
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var exchange = _rabbitMQettings.OrderExchangeName;
            var queue = _rabbitMQettings.EmailQueueName;
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true);
            await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queue, exchange, queue);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                try
                {
                    var emailEvent = JsonConvert.DeserializeObject<EmailDto>(message);
                    if(emailEvent != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        await emailService.SendEmailAsync(emailEvent);

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
