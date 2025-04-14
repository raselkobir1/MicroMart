namespace Cart.API.Domain.Dtos
{
    public class CartItemDto
    {
        public string ProductId { get; set; } = default!;
        public string InventoryId { get; set; } = default!;
        public int Quantity { get; set; }
    }

}
