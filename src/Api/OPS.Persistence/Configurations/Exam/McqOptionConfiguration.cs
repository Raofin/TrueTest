using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public class McqOptionConfiguration : IEntityTypeConfiguration<McqOption>
{
    public void Configure(EntityTypeBuilder<McqOption> entity)
    {
        entity.ToTable("McqOptions", "Exam");
        entity.HasKey(e => e.McqOptionId);

        entity.Property(e => e.McqOptionId).ValueGeneratedNever();
        entity.Property(e => e.OptionMarkdown).IsRequired();

        entity.HasOne(d => d.Question)
            .WithMany(p => p.McqQptions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}