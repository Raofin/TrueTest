using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Enum;

public class ProgLanguageConfiguration : IEntityTypeConfiguration<ProgLanguage>
{
    public void Configure(EntityTypeBuilder<ProgLanguage> entity)
    {
        entity.ToTable("ProgLanguages", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Language).IsUnique();

        entity.Property(e => e.Language).IsRequired().HasMaxLength(50);

        new BaseEntityConfig<ProgLanguage>().Configure(entity);
    }
}