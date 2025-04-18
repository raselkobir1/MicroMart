using Order.API.Domain.Entities;
using Order.API.Helper.Enums;

namespace Order.API.Domain.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public long? InventoryId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
