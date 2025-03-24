using FluentValidation;
using User.API.Helper.Enums;

namespace User.API.Domain.Dtos
{
    public class UserAddDto
    {
        public long AuthUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class UserAddDtoValidator : AbstractValidator<UserAddDto>
    {
        public UserAddDtoValidator()
        {
            RuleFor(obj => obj.AuthUserId).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.Email).EmailAddress();
            RuleFor(obj => obj.Address).MaximumLength(500);
        }
    }
} 
