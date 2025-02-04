using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Core;

namespace OPS.Persistence.Configurations.Core;

public class CloudFileConfiguration : IEntityTypeConfiguration<CloudFile>
{
    public void Configure(EntityTypeBuilder<CloudFile> entity)
    {
        entity.ToTable("CloudFiles", "Core");
        entity.HasKey(e => e.CloudFileId);

        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ContentType).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Link).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.Account)
            .WithMany(p => p.CloudFiles)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}