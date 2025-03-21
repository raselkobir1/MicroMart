using Inventory.API.Domain.Entities;

namespace Inventory.API.Domain.Dtos
{
    public class InventoryInfoDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public List<InventoryHistory> InventoryHistory { get; set; } = new();
    }
}
