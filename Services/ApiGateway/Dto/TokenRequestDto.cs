namespace ApiGateway.Dto
{
    public class TokenRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
    }
}
