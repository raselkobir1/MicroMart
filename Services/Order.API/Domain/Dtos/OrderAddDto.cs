using FluentValidation;
using Order.API.Helper.Enums;

namespace Order.API.Domain.Dtos
{
    public class OrderAddDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string CartSessionId { get; set; }
    }

    public class ProductAddDtoValidator : AbstractValidator<OrderAddDto>
    {
        public ProductAddDtoValidator()
        {
            RuleFor(obj => obj.UserId).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.UserName).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.UserEmail).EmailAddress().MaximumLength(500);
            RuleFor(obj => obj.CartSessionId).NotEmpty();
        }
    }
} 
