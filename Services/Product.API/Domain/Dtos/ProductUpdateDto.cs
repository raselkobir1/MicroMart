using FluentValidation;
using Product.API.Helper.Enums;

namespace Product.API.Domain.Dtos
{
    public class ProductUpdateDto: ProductAddDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public ProductStatus Status { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long? InventoryId { get; set; }
    }
    public class InventoryInfoUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public InventoryInfoUpdateDtoValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.SKU).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.Description).NotEmpty().MaximumLength(500);
        }
    }
}
