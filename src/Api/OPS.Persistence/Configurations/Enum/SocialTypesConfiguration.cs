using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class SocialTypesConfiguration : IEntityTypeConfiguration<SocialType>
{
    public void Configure(EntityTypeBuilder<SocialType> entity)
    {
        entity.ToTable("SocialTypes", "Enum");
        entity.HasKey(e => e.SocialTypeId);
        entity.HasIndex(e => e.PlatformName).IsUnique();
        entity.Property(e => e.PlatformName).IsRequired().HasMaxLength(255);
    }
}