namespace Auth.WebAPI.Domain.Dto.Login
{
    public class AuthResponse
    {
        public string JWTToken { get; set; }
        public DateTime JWTExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpires { get; set; }
        public string? UserOtpSecret { get; set; }
    }
}
