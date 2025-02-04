using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Configurations.Exam;

public class ExaminationConfiguration : IEntityTypeConfiguration<Examination>
{
    public void Configure(EntityTypeBuilder<Examination> entity)
    {
        entity.ToTable("Examinations", "Exam");
        entity.HasKey(e => e.ExaminationId);

        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.OpensAt).HasColumnType("datetime");
        entity.Property(e => e.ClosesAt).HasColumnType("datetime");
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
    }
}