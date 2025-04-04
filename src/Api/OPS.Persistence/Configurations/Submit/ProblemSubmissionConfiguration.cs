using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Configurations.Common;
using static OPS.Persistence.Configurations.Common.Constants;

namespace OPS.Persistence.Configurations.Submit;

public class ProblemSubmissionConfiguration : IEntityTypeConfiguration<ProblemSubmission>
{
    public void Configure(EntityTypeBuilder<ProblemSubmission> entity)
    {
        entity.ToTable("ProblemSubmissions", "Submit");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Code).IsRequired();
        entity.Property(e => e.Score).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.IsFlagged).HasDefaultValue(false);

        new BaseEntityConfig<ProblemSubmission>().Configure(entity);

        entity.HasOne(d => d.ProgLanguages)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.ProgLanguageId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Account)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Question)
            .WithMany(p => p.ProblemSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}