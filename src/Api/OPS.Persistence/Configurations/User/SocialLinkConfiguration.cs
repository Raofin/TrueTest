using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> entity)
    {
        entity.ToTable("SocialLinks", "User");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Link).IsRequired().HasMaxLength(255);

        new BaseEntityConfig<SocialLink>().Configure(entity);

        entity.HasOne(d => d.SocialType)
            .WithMany(p => p.SocialLinks)
            .HasForeignKey(d => d.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}