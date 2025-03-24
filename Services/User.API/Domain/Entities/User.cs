using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Helper.Enums;

namespace User.API.Domain.Entities
{ 
    public class User: BaseEntity
    {
        public long AuthUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; } 
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
    public class ProductConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("Users", "user");   

            builder.Property(t => t.Name).HasMaxLength(200);
        }
    }
}
