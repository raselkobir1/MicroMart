using FluentValidation;
using Order.API.Domain.Dto.Common.PaginatedResult;

namespace Order.API.Domain.Dtos
{
    public class OrderFilterDto: BaseFilterDto
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; } 
    }
    public class OrderFilterDtoValidator : AbstractValidator<OrderFilterDto>
    {
        public OrderFilterDtoValidator()
        {
            Include(new BaseFilterDtoValidator());
        }
    }
}
