namespace Order.API.MessageBroker
{
    public interface IRabbitMQMessageProducer
    {
        Task SendMessageToQueue<T>(string exchange,string queue, T message);
    }
}
