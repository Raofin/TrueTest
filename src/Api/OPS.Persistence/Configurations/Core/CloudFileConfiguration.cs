using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Core;

public class CloudFileConfiguration : IEntityTypeConfiguration<CloudFile>
{
    public void Configure(EntityTypeBuilder<CloudFile> entity)
    {
        entity.ToTable("CloudFiles", "Core");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ContentType).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Link).IsRequired();

        new BaseEntityConfig<CloudFile>().Configure(entity);
        new SoftDeletableEntityConfig<CloudFile>().Configure(entity);

        entity.HasOne(d => d.Profile)
            .WithOne(p => p.ImageFile)
            .HasForeignKey<Profile>(d => d.ImageFileId);
    }
}