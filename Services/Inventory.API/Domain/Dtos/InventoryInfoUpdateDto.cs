using FluentValidation;
using Inventory.API.Helper.Enums;

namespace Inventory.API.Domain.Dtos
{
    public class InventoryInfoUpdateDto: InventoryInfoAddDto
    {
        public long Id { get; set; }
        public ActionType ActionType { get; set; } 
    }
    public class InventoryInfoUpdateDtoValidator : AbstractValidator<InventoryInfoUpdateDto>
    {
        public InventoryInfoUpdateDtoValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.ProductId).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().MaximumLength(200);
            RuleFor(obj => obj.SKU).NotEmpty().MaximumLength(50);
            RuleFor(obj => obj.Description).NotEmpty().MaximumLength(500);
        }
    }
}
