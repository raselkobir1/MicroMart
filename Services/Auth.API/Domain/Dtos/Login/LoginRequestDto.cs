using System.ComponentModel.DataAnnotations;

namespace Auth.WebAPI.Domain.Dto.Login
{
    public class LoginRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
