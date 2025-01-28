using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class FlaggedSolutionConfiguration : IEntityTypeConfiguration<FlaggedSolution>
{
    public void Configure(EntityTypeBuilder<FlaggedSolution> entity)
    {
        // Table
        entity.ToTable("FlaggedSolution", "exam");
        entity.HasKey(e => e.FlaggedSolutionId);

        // Properties
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.Reason).IsRequired();

        // Relationships
        entity.HasOne(d => d.UserSolution).WithMany(p => p.FlaggedSolutions)
            .HasForeignKey(d => d.UserSolutionId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
