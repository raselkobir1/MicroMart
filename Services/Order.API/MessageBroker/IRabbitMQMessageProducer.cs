namespace Order.API.MessageBroker
{
    public interface IRabbitMQMessageProducer
    {
        Task SendMessageToQueue<T>(string queue, T message);
    }
}
