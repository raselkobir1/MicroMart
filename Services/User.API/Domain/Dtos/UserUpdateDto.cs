using FluentValidation;
using User.API.Helper.Enums;

namespace User.API.Domain.Dtos
{
    public class UserUpdateDto: UserAddDto
    {
        public long Id { get; set; }
        public long AuthUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.AuthUserId).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.Email);
            RuleFor(obj => obj.Address).MaximumLength(500);
        }
    }
}
