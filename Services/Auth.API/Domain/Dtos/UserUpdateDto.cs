using FluentValidation;
using Auth.API.Helper.Enums;

namespace Auth.API.Domain.Dtos
{
    public class UserUpdateDto: UserAddDto
    {
        public long Id { get; set; }
    }
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.UserName).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.Email).EmailAddress().NotEmpty();
        }
    }
}
