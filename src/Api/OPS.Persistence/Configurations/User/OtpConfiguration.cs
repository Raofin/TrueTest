using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> entity)
    {
        entity.ToTable("Otps", "User");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Code).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ExpiresAt)
            .HasDefaultValueSql("(DateAdd(minute, (5), GetUtcDate()))")
            .HasColumnType("DateTime");

        new BaseEntityConfig<Otp>().Configure(entity);
    }
}