using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.API.Domain.Entities
{
    public class InventoryInfo: BaseEntity
    {
        public long ProductId { get; set; } 
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        #region Navigation Properties
        public List<InventoryHistory> InventoryHistory { get; set; }
        #endregion
    }
    public class InventoryConfiguration : BaseEntityTypeConfiguration<InventoryInfo>
    {
        public override void Configure(EntityTypeBuilder<InventoryInfo> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryInfos", "inventory");  

            builder.Property(t => t.Name).HasMaxLength(200);
        }
    }
}
