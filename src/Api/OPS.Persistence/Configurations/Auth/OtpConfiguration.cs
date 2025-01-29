using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Configurations.Auth;

public partial class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> entity)
    {
        // Table
        entity.ToTable("Otps", "auth");
        entity.HasKey(e => e.Email);

        // Properties
        entity.Property(e => e.Email).HasMaxLength(255);
        entity.Property(e => e.Code).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ExpiresAt).HasDefaultValueSql("(dateadd(minute,(5),getdate()))").HasColumnType("datetime");
    }
}
