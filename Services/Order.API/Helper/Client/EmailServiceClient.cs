
namespace Order.API.Helper.Client
{
    public class EmailServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailServiceClient> _logger;

        public EmailServiceClient(HttpClient httpClient, ILogger<EmailServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SendEmail/Send", emailSend);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("Failed to send email verification code");
            return false;
        }
    }
    public class EmailSendDto
    {
        public EmailSendDto()
        {
            To = new List<string>();
            Cc = new List<string>();
        }
        public List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
