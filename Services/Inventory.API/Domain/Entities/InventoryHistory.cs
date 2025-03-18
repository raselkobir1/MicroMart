using Inventory.API.Helper.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.API.Domain.Entities
{
    public class InventoryHistory : BaseEntity
    {
        public long InventoryInfoId { get; set; } 
        public string SKU { get; set; }
        public ActionType ActionType { get; set; }
        public int QuentityChanged { get; set; }
        public int LastQuentity { get; set; }
        public int NewQuentity { get; set; }

        #region Navigation Properties
        public virtual InventoryInfo InventoryInfo { get; set; }
        #endregion
    }
    public class HistoryConfiguration : BaseEntityTypeConfiguration<InventoryHistory>
    {
        public override void Configure(EntityTypeBuilder<InventoryHistory> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryHistory", "inventory");

            builder.Property(t => t.SKU).HasMaxLength(200);
        }
    }
}
