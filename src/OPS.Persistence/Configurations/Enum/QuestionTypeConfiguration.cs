using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public partial class QuestionTypeConfiguration : IEntityTypeConfiguration<QuestionType>
{
    public void Configure(EntityTypeBuilder<QuestionType> entity)
    {
        // Table
        entity.ToTable("QuestionTypes", "enum");
        entity.HasKey(e => e.QuestionTypeId);

        // Properties
        entity.Property(e => e.Type).IsRequired().HasMaxLength(255);
    }
}
