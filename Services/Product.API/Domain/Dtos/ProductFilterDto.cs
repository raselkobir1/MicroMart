using FluentValidation;
using Product.API.Domain.Dto.Common.PaginatedResult;

namespace Product.API.Domain.Dtos
{
    public class ProductFilterDto: BaseFilterDto
    {
        public string? Name { get; set; }
        public string? SKU { get; set; } 
    }
    public class ProductFilterDtoValidator : AbstractValidator<ProductFilterDto>
    {
        public ProductFilterDtoValidator()
        {
            Include(new BaseFilterDtoValidator());
        }
    }
}
