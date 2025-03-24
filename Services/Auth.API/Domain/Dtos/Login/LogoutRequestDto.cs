using System.ComponentModel.DataAnnotations;

namespace Auth.WebAPI.Domain.Dto.Login
{
    public class LogoutRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; }
    }
}
