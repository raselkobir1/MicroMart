using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using ApiGateway.Dto;

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

        public async Task<AuthorizeResponse?> GetToken(TokenRequestDto dto)
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
    }

}
