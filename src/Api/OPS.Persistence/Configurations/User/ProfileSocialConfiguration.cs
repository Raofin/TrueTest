using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Configurations.User;

public class ProfileSocialConfiguration : IEntityTypeConfiguration<ProfileSocial>
{
    public void Configure(EntityTypeBuilder<ProfileSocial> entity)
    {
        entity.ToTable("ProfileSocials", "User");
        entity.HasKey(e => new { AccountId = e.ProfileId, e.SocialLinkId });

        entity.HasOne(d => d.Profile)
            .WithMany(p => p.ProfileSocials)
            .HasForeignKey(d => d.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.SocialLink)
            .WithMany(p => p.ProfileSocials)
            .HasForeignKey(d => d.SocialLinkId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}