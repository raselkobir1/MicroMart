using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Helper.Enums;

namespace Order.API.Domain.Entities
{ 
    public class Order: BaseEntity
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public OrderStatus Status { get; set; } 
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        #region Navigation Properties
        public List<OrderItem> OrderItems { get; set; }
        #endregion
    }

    public class ProductConfiguration : BaseEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.ToTable("Orders", "order");   
        }
    }
}
