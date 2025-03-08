using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Submit;

public class TestCaseOutputConfiguration : IEntityTypeConfiguration<TestCaseOutput>
{
    public void Configure(EntityTypeBuilder<TestCaseOutput> entity)
    {
        entity.ToTable("TestCaseOutputs", "Submit");
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Output).IsRequired();
        entity.Property(e => e.IsAccepted).HasDefaultValue(false);

        new BaseEntityConfig<TestCaseOutput>().Configure(entity);

        entity.HasOne(d => d.TestCase)
            .WithMany(p => p.TestCaseOutputs)
            .HasForeignKey(d => d.TestCaseId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.ProblemSubmissions)
            .WithMany(p => p.TestCaseOutputs)
            .HasForeignKey(d => d.ProblemSubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}