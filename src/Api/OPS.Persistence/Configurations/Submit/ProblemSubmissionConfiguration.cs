using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Configurations.Submit;

public class ProblemSubmissionConfiguration : IEntityTypeConfiguration<ProblemSubmission>
{
    public void Configure(EntityTypeBuilder<ProblemSubmission> entity)
    {
        entity.ToTable("ProblemSubmissions", "Submit");
        entity.HasKey(e => e.ProblemSubmissionId);

        entity.Property(e => e.Code).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.Account)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Question)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.ProgLanguages)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.ProgLanguagesId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}