namespace Order.API.Domain.Dtos
{
    public class OrderItemDto
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; } = 0;
    }
}
