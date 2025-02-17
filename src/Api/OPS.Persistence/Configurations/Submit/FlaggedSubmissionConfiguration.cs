using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Submit;

public class FlaggedSubmissionConfiguration : IEntityTypeConfiguration<FlaggedSubmission>
{
    public void Configure(EntityTypeBuilder<FlaggedSubmission> entity)
    {
        entity.ToTable("FlaggedSubmissions", "Submit");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.ReasonMarkdown).IsRequired();

        new BaseEntityConfig<FlaggedSubmission>().Configure(entity);
        new SoftDeletableEntityConfig<FlaggedSubmission>().Configure(entity);

        entity.HasOne(d => d.ProblemSubmission)
            .WithMany(p => p.FlaggedSubmissions)
            .HasForeignKey(d => d.ProblemSubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}