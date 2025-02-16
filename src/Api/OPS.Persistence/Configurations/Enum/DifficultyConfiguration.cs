using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class DifficultyConfiguration
{
    public void Configure(EntityTypeBuilder<Difficulty> entity)
    {
        entity.ToTable("Difficulties", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.DifficultyName).IsUnique();

        entity.Property(e => e.DifficultyName).IsRequired().HasMaxLength(50);
    }
}