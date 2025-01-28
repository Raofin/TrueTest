using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> entity)
    {
        // Table
        entity.ToTable("TestCases", "exam");
        entity.HasKey(e => e.TestCaseId);

        // Properties
        entity.Property(e => e.Input).IsRequired();
        entity.Property(e => e.Output).IsRequired();

        // Relationships
        entity.HasOne(d => d.Problem).WithMany(p => p.TestCases)
            .HasForeignKey(d => d.ProblemId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
