using Inventory.API.Domain.Entities.SharedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.API.Domain.Entities
{
    public class Product: BaseEntity
    {
        public long Id { get; set; } 
        public string Name { get; set; } 
        public string SKU { get; set; }  
        public string Description { get; set; }   
    }
    public class ProductConfiguration : BaseEntityTypeConfiguration<Product>
    {

        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            builder.ToTable("Products","product");
        }
    }
}
