using Product.API.Domain.Entities;
using Product.API.Helper.Enums;

namespace Product.API.Domain.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public long? InventoryId { get; set; }
        public int Quantity { get; set; }
        public string InvantoryStatus { get; set; }
    }
}
