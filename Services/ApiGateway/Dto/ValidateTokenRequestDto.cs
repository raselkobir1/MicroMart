namespace ApiGateway.Dto
{
    public class ValidateTokenRequestDto
    {
        public required string Token { get; set; }
        public required string Realm { get; set; } = string.Empty;
    }
}
