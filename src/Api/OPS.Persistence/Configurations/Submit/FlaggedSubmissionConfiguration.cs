using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Configurations.Submit;

public class FlaggedSubmissionConfiguration : IEntityTypeConfiguration<FlaggedSubmission>
{
    public void Configure(EntityTypeBuilder<FlaggedSubmission> entity)
    {
        entity.ToTable("FlaggedSubmissions", "Exam");
        entity.HasKey(e => e.FlaggedSolutionId);

        entity.Property(e => e.ReasonMarkdown).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.ProblemSubmission)
            .WithMany(p => p.FlaggedSubmissions)
            .HasForeignKey(d => d.ProblemSubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}