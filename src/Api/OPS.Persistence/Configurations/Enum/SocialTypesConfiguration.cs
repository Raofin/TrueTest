using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Enum;

public class SocialTypesConfiguration : IEntityTypeConfiguration<SocialType>
{
    public void Configure(EntityTypeBuilder<SocialType> entity)
    {
        entity.ToTable("SocialTypes", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.PlatformName).IsUnique();

        entity.Property(e => e.PlatformName).IsRequired().HasMaxLength(255);

        new BaseEntityConfig<SocialType>().Configure(entity);
    }
}