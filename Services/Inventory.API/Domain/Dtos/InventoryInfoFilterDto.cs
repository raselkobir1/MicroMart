using FluentValidation;
using Inventory.API.Domain.Dto.Common.PaginatedResult;

namespace Inventory.API.Domain.Dtos
{
    public class InventoryInfoFilterDto: BaseFilterDto
    {
        public string? Name { get; set; }
        public string? SKU { get; set; } 
    }
    public class InventoryInfoFilterDtoValidator : AbstractValidator<InventoryInfoFilterDto>
    {
        public InventoryInfoFilterDtoValidator()
        {
            Include(new BaseFilterDtoValidator());
        }
    }
}
