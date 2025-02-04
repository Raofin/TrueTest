using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> entity)
    {
        entity.ToTable("Questions", "Exam");
        entity.HasKey(e => e.QuestionId);

        entity.Property(e => e.StatementMarkdown).IsRequired();
        entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.HasOne(d => d.Difficulty)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.DifficultyId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Examination)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.ExaminationId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.QuestionType)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}