using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Configurations.Auth;

public class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> entity)
    {
        entity.ToTable("Otps", "Auth");
        entity.HasKey(e => e.OtpId);

        entity.Property(e => e.Code).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ExpiresAt)
            .HasDefaultValueSql("(dateadd(minute,(5),getutcdate()))")
            .HasColumnType("datetime");
    }
}