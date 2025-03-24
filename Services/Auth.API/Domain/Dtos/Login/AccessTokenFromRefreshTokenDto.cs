using System.ComponentModel.DataAnnotations;

namespace Auth.WebAPI.Domain.Dto.Login
{
    public class AccessTokenFromRefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
