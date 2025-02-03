using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Configurations.User;

public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> entity)
    {
        entity.ToTable("SocialLinks", "User");
        entity.HasKey(e => e.SocialLinkId);

        entity.Property(e => e.Link).IsRequired().HasMaxLength(255);

        entity.HasOne(d => d.SocialType)
            .WithMany(p => p.SocialLinks)
            .HasForeignKey(d => d.SocialTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}