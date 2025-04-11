using ApiGateway.Dto;
using System.Text.Json;

namespace ApiGateway.Helper.Client
{
    public class ValidateAuthorizeClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ValidateAuthorizeClient> _logger;

        public ValidateAuthorizeClient(HttpClient httpClient, ILogger<ValidateAuthorizeClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AuthorizeResponse> ValidateAuthorizeAsync(string jwtToken)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Login/ValidateToken", jwtToken);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    _logger.LogError("Response body is empty");
                    return new AuthorizeResponse();
                }

                var authResponse = JsonSerializer.Deserialize<AuthorizeResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                authResponse.IsSuccess = true;
                return authResponse;
            }
            else
            {
                _logger.LogError($"Token validation failed. Status: {response.StatusCode}");
                return new AuthorizeResponse();
            }

        }
    }
}
