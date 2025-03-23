using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Auth.API.Helper.Enums;

namespace Auth.API.Domain.Entities
{ 
    public class User: BaseEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public AccountStatus Status { get; set; } 
        public bool Verified { get; set; }
    }

    public class UserConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder); 
            builder.ToTable("Users", "user");   

            builder.Property(t => t.UserName).HasMaxLength(50);
        }
    }
}
