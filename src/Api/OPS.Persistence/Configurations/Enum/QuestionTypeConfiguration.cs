using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class QuestionTypeConfiguration
{
    public void Configure(EntityTypeBuilder<QuestionType> entity)
    {
        entity.ToTable("QuestionTypes", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Type).IsUnique();

        entity.Property(e => e.Type).IsRequired().HasMaxLength(255);
    }
}