using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Exam;

public class ExamCandidateConfiguration : IEntityTypeConfiguration<ExamCandidate>
{
    public void Configure(EntityTypeBuilder<ExamCandidate> entity)
    {
        entity.ToTable("ExamCandidates", "Exam");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.CandidateEmail).IsRequired().HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        new BaseEntityConfig<ExamCandidate>().Configure(entity);
        new SoftDeletableEntityConfig<ExamCandidate>().Configure(entity);

        entity.HasOne(d => d.Account)
            .WithMany(p => p.ExamCandidates)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Examination)
            .WithMany(p => p.ExamCandidates)
            .HasForeignKey(d => d.ExaminationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}