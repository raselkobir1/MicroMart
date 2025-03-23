using Auth.API.Domain.Entities;
using Auth.API.Helper.Enums;

namespace Auth.API.Domain.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
