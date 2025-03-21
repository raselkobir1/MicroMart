using FluentValidation;
using Product.API.Helper.Enums;

namespace Product.API.Domain.Dtos
{
    public class ProductAddDto
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public ProductStatus Status { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long? InventoryId { get; set; }
    }

    public class ProductAddDtoValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddDtoValidator()
        {
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.SKU).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.Description).MaximumLength(500);
        }
    }
} 
