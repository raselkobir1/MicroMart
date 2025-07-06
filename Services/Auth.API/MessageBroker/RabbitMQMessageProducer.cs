using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Auth.API.Domain.Dtos.Common;
using RabbitMQ.Client;
using System.Text;

namespace Auth.API.MessageBroker
{
    public class RabbitMQMessageProducer : IRabbitMQMessageProducer
    {
        private readonly RabbitMqSettings _rabbitMQettings;
        public RabbitMQMessageProducer(IOptions<RabbitMqSettings> settings)
        {
            _rabbitMQettings = settings.Value;
        }
        public async Task SendMessageToQueue<T>(string queue,string exchange, T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQettings.HostName,
                Port     = _rabbitMQettings.Port,
                UserName = _rabbitMQettings.UserName,
                Password = _rabbitMQettings.Password
            };

            //var exchange = _rabbitMQettings.AuthExchangeName;

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Ensure exchange and queue exist (optional if consumer declares them first)
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true);
            await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queue, exchange, routingKey: queue);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            await channel.BasicPublishAsync(exchange: exchange, routingKey: queue, body: body);
            await connection.CloseAsync();

            Console.WriteLine($"✅ Sent {typeof(T).Name} message to '{queue}' queue.");
        }
    }
}
