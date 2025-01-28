using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public partial class SocialPlatformConfiguration : IEntityTypeConfiguration<SocialPlatform>
{
    public void Configure(EntityTypeBuilder<SocialPlatform> entity)
    {
        // Table
        entity.ToTable("SocialPlatforms", "enum");
        entity.HasKey(e => e.SocialPlatformId);
        entity.HasIndex(e => e.PlatformName).IsUnique();

        // Properties
        entity.Property(e => e.PlatformName).IsRequired().HasMaxLength(255);
    }
}
