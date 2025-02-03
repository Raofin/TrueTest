using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Core;

namespace OPS.Persistence.Configurations.Core;

public class LogEventConfiguration : IEntityTypeConfiguration<LogEvent>
{
    public void Configure(EntityTypeBuilder<LogEvent> entity)
    {
        entity.ToTable("LogEvents", "Core");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Message).IsRequired();
        entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
        entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
    }
}