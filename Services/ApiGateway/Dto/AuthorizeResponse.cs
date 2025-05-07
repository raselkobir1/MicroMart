using System.Text.Json.Serialization;

namespace ApiGateway.Dto
{
    public class AuthorizeResponse
    {
        public bool IsSuccess { get; set; } = false;
        public AuthorizeData Data { get; set; }
        public TokenResponse TokenInfo { get; set; }
        public string Message { get; set; }
    }

    public class AuthorizeData
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }


    }
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
