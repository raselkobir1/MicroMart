using User.API.Domain.Entities;
using User.API.Helper.Enums;

namespace User.API.Domain.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        public long AuthUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
