using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Domain.Entities
{
    public class LoginHistory 
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
    public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
    {
        public void Configure(EntityTypeBuilder<LoginHistory> builder)
        {
            builder.ToTable("LoginHistory", "user");
            builder.HasKey(t => t.Id);
        }
    }
}
