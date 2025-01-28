using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class McqOptionConfiguration : IEntityTypeConfiguration<McqOption>
{
    public void Configure(EntityTypeBuilder<McqOption> entity)
    {
        // Table
        entity.ToTable("McqOptions", "exam");
        entity.HasKey(e => e.McqOptionId);

        // Properties
        entity.Property(e => e.Optionn).IsRequired();
    }
}
