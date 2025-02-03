using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class ProgLanguageConfiguration : IEntityTypeConfiguration<ProgLanguage>
{
    public void Configure(EntityTypeBuilder<ProgLanguage> entity)
    {
        entity.ToTable("ProgLanguages", "Enum");
        entity.HasKey(e => e.ProgLanguageId);
        entity.HasIndex(e => e.Language).IsUnique();
        entity.Property(e => e.Language).IsRequired().HasMaxLength(50);
    }
}