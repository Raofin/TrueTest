using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public partial class ExaminationConfiguration : IEntityTypeConfiguration<Examination>
{
    public void Configure(EntityTypeBuilder<Examination> entity)
    {
        // Table
        entity.ToTable("Examinations", "exam");
        entity.HasKey(e => e.ExamId);

        // Properties
        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.OpensAt).HasColumnType("datetime");
        entity.Property(e => e.ClosesAt).HasColumnType("datetime");
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
    }
}
