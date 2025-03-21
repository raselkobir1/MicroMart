using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Helper.Enums;

namespace Product.API.Domain.Entities
{ 
    public class Product: BaseEntity
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public ProductStatus Status { get; set; } 
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long? InventoryId { get; set; } 
    }
    public class ProductConfiguration : BaseEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            builder.ToTable("Products", "pro");   

            builder.Property(t => t.Name).HasMaxLength(200);
        }
    }
}
