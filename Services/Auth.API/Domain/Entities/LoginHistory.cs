using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Domain.Entities
{
    public class LoginHistory : BaseEntity
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

        public User User { get; set; }
    }
    public class LoginHistoryConfiguration : BaseEntityTypeConfiguration<LoginHistory>
    {
        public override void Configure(EntityTypeBuilder<LoginHistory> builder)
        {
            base.Configure(builder);
            builder.ToTable("LoginHistory", "user");
        }
    }
}
