
using Auth.API.Domain.Dtos.Common;

namespace Auth.API.Helper.Client
{
    public class SendVerificationEmailClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SendVerificationEmailClient> _logger;

        public SendVerificationEmailClient(HttpClient httpClient, ILogger<SendVerificationEmailClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> SendVerificationCodeAsync(EmailSendDto sendDto)  
        {
            var response = await _httpClient.PostAsJsonAsync("api/SendEmail/Send", sendDto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("Failed to send email verification code");
            return false;
        }
    }

}
