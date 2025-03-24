using FluentValidation;
using Auth.API.Helper.Enums;

namespace Auth.API.Domain.Dtos
{
    public class UserAddDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }

    public class UserAddDtoValidator : AbstractValidator<UserAddDto>
    {
        public UserAddDtoValidator()
        {
            RuleFor(obj => obj.UserName).NotEmpty();
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty();
        }
    }
} 
