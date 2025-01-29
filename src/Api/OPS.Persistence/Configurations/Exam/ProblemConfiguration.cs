using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class ProblemConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> entity)
    {
        // Table
        entity.ToTable("Problems", "exam");
        entity.HasKey(e => e.ProblemId);

        // Properties
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.DifficultyLevel).IsRequired().HasMaxLength(50);
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        // Relationships
        entity.HasOne(d => d.Question)
            .WithMany(p => p.Problems)
            .HasForeignKey(d => d.QuestionId);
    }
}
