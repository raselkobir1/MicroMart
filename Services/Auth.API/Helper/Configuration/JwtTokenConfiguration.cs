using System.ComponentModel.DataAnnotations;

namespace Auth.API.Helper.Configuration
{
    public class JwtTokenConfiguration
    {
        [Required(AllowEmptyStrings = false)]
        public string Issuer { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Audience { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SigningKey { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JWTTokenExpirationMinutes { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string RefreshTokenExpirationMinutes { get; set; }
    }
}
