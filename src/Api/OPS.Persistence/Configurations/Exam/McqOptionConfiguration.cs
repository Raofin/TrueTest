using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Exam;

public class McqOptionConfiguration : IEntityTypeConfiguration<McqOption>
{
    public void Configure(EntityTypeBuilder<McqOption> entity)
    {
        entity.ToTable("McqOption", "Exam");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Option1).IsRequired();
        entity.Property(e => e.Option2).IsRequired();
        entity.Property(e => e.AnswerOptions).IsRequired().HasMaxLength(50);
        entity.Property(e => e.IsMultiSelect).HasDefaultValue(false);

        new BaseEntityConfig<McqOption>().Configure(entity);

        entity.HasOne(d => d.Question)
            .WithOne(p => p.McqOption)
            .HasForeignKey<McqOption>(d => d.QuestionId);
    }
}