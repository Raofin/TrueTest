using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> entity)
    {
        // Table
        entity.ToTable("Questions", "exam");
        entity.HasKey(e => e.QuestionId);

        // Properties
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.Statement).IsRequired();
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        // Relationships
        entity.HasOne(d => d.Exam).WithMany(p => p.Questions)
            .HasForeignKey(d => d.ExamId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
            .HasForeignKey(d => d.QuestionTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        entity.HasMany(d => d.McqOptions).WithMany(p => p.Questions)
            .UsingEntity<Dictionary<string, object>>("McqAnswer",
                r => r.HasOne<McqOption>().WithMany()
                        .HasForeignKey("McqOptionId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                l => l.HasOne<Question>().WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("QuestionId", "McqOptionId");
                    j.ToTable("McqAnswers", "exam");
                });
    }
}
