using FluentValidation;

namespace Auth.API.Domain.Dtos
{
    public class UserVerificationDto
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
    public class UserVerificationDtoValidator : AbstractValidator<UserVerificationDto>
    {
        public UserVerificationDtoValidator()
        {
            RuleFor(obj => obj.VerificationCode).NotEmpty();
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
        }
    }
}
