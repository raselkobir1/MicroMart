namespace Order.API.Domain.Dtos.Common
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string OrderExchangeName { get; set; }
        public string EmailQueueName { get; set; }
        public string ClearCartQueueName { get; set; } 
    }
}
