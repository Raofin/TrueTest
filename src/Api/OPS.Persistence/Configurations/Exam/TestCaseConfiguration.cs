using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> entity)
    {
        entity.ToTable("TestCases", "Exam");
        entity.HasKey(e => e.TestCaseId);

        entity.Property(e => e.Input).IsRequired();
        entity.Property(e => e.Output).IsRequired();

        entity.HasOne(d => d.Question)
            .WithMany(q => q.TestCases)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}