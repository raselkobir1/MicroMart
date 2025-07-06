namespace Auth.API.Domain.Dtos.Common
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string UserProfileExchangeName { get; set; } 
        public string UserProfileQueueName { get; set; } 
        public string EmailQueueName { get; set; }
        public string EmailExchangeName { get; set; }
    }
}
