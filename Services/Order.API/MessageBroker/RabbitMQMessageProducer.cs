using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Order.API.MessageBroker
{
    public class RabbitMQMessageProducer: IRabbitMQMessageProducer
    {
        public RabbitMQMessageProducer()
        {
        }

        public async Task SendMessageToQueue<T>(string queue, T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var exchange = "order";
            await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Direct, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            await channel.BasicPublishAsync(exchange: exchange, routingKey: queue, body: body);

            Console.WriteLine($"Sent {message} to {queue}");
        }
    }
}
