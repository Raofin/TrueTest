using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class ExamCandidateConfiguration : IEntityTypeConfiguration<ExamCandidate>
{
    public void Configure(EntityTypeBuilder<ExamCandidate> entity)
    {
        // Table
        entity.ToTable("ExamCandidates", "exam");
        entity.HasKey(e => e.ExamCandidateId);

        // Properties
        entity.Property(e => e.UserEmail).IsRequired().HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        // Relationships
        entity.HasOne(d => d.Exam)
            .WithMany(p => p.ExamCandidates)
            .HasForeignKey(d => d.ExamId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
