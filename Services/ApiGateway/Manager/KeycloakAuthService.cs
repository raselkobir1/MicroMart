using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using ApiGateway.Dto;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace ApiGateway.Manager
{
    public class KeycloakAuthService: IKeycloakAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _options;

        public KeycloakAuthService(HttpClient httpClient, IOptions<KeycloakOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<AuthorizeResponse> GetToken(TokenRequestDto dto)
        {
            if (!_options.Realms.TryGetValue(dto.Realm, out var realmConfig))
            {
                throw new ArgumentException($"Realm '{dto.Realm}' is not configured.");
            }

            var tokenUrl = $"{_options.ServerUrl}/realms/{realmConfig.Realm}/protocol/openid-connect/token";

            var form = new Dictionary<string, string>
                       {
                            { "grant_type", "password" },
                            { "client_id", realmConfig.ClientId },
                            { "client_secret", realmConfig.ClientSecret },
                            { "username", dto.UserName },
                            { "password", dto.Password }
                       };

            var response = await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak login failed: {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenResponse>(content);
            var authorizeResponse = new AuthorizeResponse();
            if (token != null)
            {
                authorizeResponse.TokenInfo = token;
                authorizeResponse.IsSuccess = true;
            }
            return authorizeResponse;
        }

        public async Task<AuthorizeResponse> ValidateToken(ValidateTokenRequestDto dto)
        {
            if (!_options.Realms.TryGetValue(dto.Realm, out var realmConfig))
            {
                throw new ArgumentException($"Realm '{dto.Realm}' is not configured.");
            }
            var introspectUrl = $"{_options.ServerUrl}/realms/{realmConfig.Realm}/protocol/openid-connect/token/introspect";

            var request = new HttpRequestMessage(HttpMethod.Post, introspectUrl);

            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{realmConfig.ClientId}:{realmConfig.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", dto.Token)
            });
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"Invalid token.");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            return new AuthorizeResponse();
        }
    }
}
