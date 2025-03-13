using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Exam;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> entity)
    {
        entity.ToTable("TestCases", "Exam");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Input).IsRequired();
        entity.Property(e => e.ExpectedOutput).IsRequired();

        new BaseEntityConfig<TestCase>().Configure(entity);

        entity.HasOne(d => d.Question)
            .WithMany(q => q.TestCases)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}