using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class ProfileSocialConfiguration : IEntityTypeConfiguration<ProfileSocial>
{
    public void Configure(EntityTypeBuilder<ProfileSocial> entity)
    {
        entity.ToTable("ProfileSocials", "User");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Link).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

        new BaseEntityConfig<ProfileSocial>().Configure(entity);

        entity.HasOne(d => d.Profile)
            .WithMany(p => p.ProfileSocials)
            .HasForeignKey(d => d.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}