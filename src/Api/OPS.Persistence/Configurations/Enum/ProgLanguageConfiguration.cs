using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities2.Configurations;

public partial class ProgLanguageConfiguration : IEntityTypeConfiguration<ProgLanguage>
{
    public void Configure(EntityTypeBuilder<ProgLanguage> entity)
    {
        // Table
        entity.ToTable("ProgLanguages", "enum");
        entity.HasKey(e => e.ProgLanguagesId);
        entity.HasIndex(e => e.Language).IsUnique();

        // Properties
        entity.Property(e => e.Language).IsRequired().HasMaxLength(50);
    }
}
