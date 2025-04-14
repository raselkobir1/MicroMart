
using FluentValidation;

namespace Cart.API.Domain.Dto.Common.PaginatedResult
{
    public class BaseFilterDtoValidator : AbstractValidator<BaseFilterDto>
    {
        public BaseFilterDtoValidator()
        {
            RuleFor(obj => obj.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.PageNumber).NotEmpty().GreaterThan(0);
        }
    }
}
