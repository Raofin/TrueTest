using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Core;

namespace OPS.Persistence.Configurations.Core;

public partial class CloudFileConfiguration : IEntityTypeConfiguration<CloudFile>
{
    public void Configure(EntityTypeBuilder<CloudFile> entity)
    {
        // Table
        entity.ToTable("CloudFiles", "core");
        entity.HasKey(e => e.CloudFileId);

        // Properties
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ContentType).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Link).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
    }
}
