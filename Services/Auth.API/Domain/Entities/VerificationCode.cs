using Auth.API.Helper.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Domain.Entities
{
    public class VerificationCode : BaseEntity
    {
        public long UserId { get; set; }
        public string Code { get; set; }
        public VerificationStatus Status { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public User User { get; set; }
    }
    public class VerificationCodeConfiguration : BaseEntityTypeConfiguration<VerificationCode>
    {
        public override void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("VerificationCode", "user");

            builder.Property(t => t.Code).HasMaxLength(12);
        }
    }
}
