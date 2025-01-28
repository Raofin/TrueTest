using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Core;

namespace OPS.Persistence.Configurations.Core;

public partial class LogEventConfiguration : IEntityTypeConfiguration<LogEvent>
{
    public void Configure(EntityTypeBuilder<LogEvent> entity)
    {
        // Table
        entity.ToTable("LogEvents", "core");
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.Message).IsRequired();
        entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
        entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
    }
}
