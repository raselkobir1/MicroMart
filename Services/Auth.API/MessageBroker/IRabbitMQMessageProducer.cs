namespace Auth.API.MessageBroker
{
    public interface IRabbitMQMessageProducer
    {
        Task SendMessageToQueue<T>(string queue,string exchange, T message); 
    }
}
