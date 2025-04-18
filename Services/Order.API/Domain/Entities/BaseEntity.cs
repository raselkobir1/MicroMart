using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Order.API.DataAccess.Interfaces;

namespace Order.API.Domain.Entities
{
    public class BaseEntity: ISoftDeletable
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public abstract class BaseEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
            builder.Property(t => t.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(t => t.ModifiedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.CreatedDate).HasColumnType("timestamp");
            builder.Property(e => e.ModifiedDate).HasColumnType("timestamp");
        }
    }
}
