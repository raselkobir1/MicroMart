using FluentValidation;

namespace Inventory.API.Domain.Dtos
{
    public class InventoryInfoAddDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class InventoryInfoAddDtoValidator : AbstractValidator<InventoryInfoAddDto>
    {
        public InventoryInfoAddDtoValidator()
        {
            RuleFor(obj => obj.ProductId).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.SKU).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.Description).NotEmpty().MaximumLength(500);
        }
    }
} 
