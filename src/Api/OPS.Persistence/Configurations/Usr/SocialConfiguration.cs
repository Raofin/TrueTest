using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence.Configurations.Usr;

public partial class SocialConfiguration : IEntityTypeConfiguration<Social>
{
    public void Configure(EntityTypeBuilder<Social> entity)
    {
        // Table
        entity.ToTable("Socials", "usr");
        entity.HasKey(e => e.SocialId);

        // Properties
        entity.Property(e => e.Link).IsRequired().HasMaxLength(255);

        // Relationships
        entity.HasOne(d => d.SocialPlatform)
            .WithMany(p => p.Socials)
            .HasForeignKey(d => d.SocialPlatformId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
