using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Exam;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> entity)
    {
        entity.ToTable("Questions", "Exam");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.StatementMarkdown).IsRequired();
        entity.Property(e => e.Points).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.HasLongAnswer).HasDefaultValue(false);

        new BaseEntityConfig<Question>().Configure(entity);
        new SoftDeletableEntityConfig<Question>().Configure(entity);

        entity.HasOne(d => d.QuestionType)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Difficulty)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.DifficultyId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Examination)
            .WithMany(p => p.Questions)
            .HasForeignKey(d => d.ExaminationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}