using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public class McqAnswerConfiguration : IEntityTypeConfiguration<McqAnswer>
{
    public void Configure(EntityTypeBuilder<McqAnswer> entity)
    {
        entity.ToTable("McqAnswers", "Exam");
        entity.HasKey(e => e.Id);

        entity.HasOne(d => d.McqOption)
            .WithMany(p => p.McqAnswers)
            .HasForeignKey(d => d.McqOptionId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Question)
            .WithMany(p => p.McqAnswers)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}