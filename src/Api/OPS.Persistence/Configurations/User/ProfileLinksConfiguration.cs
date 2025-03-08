using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class ProfileLinksConfiguration : IEntityTypeConfiguration<ProfileLinks>
{
    public void Configure(EntityTypeBuilder<ProfileLinks> entity)
    {
        entity.ToTable("ProfileLink" +
                       "s", "User");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Link).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

        new BaseEntityConfig<ProfileLinks>().Configure(entity);

        entity.HasOne(d => d.Profile)
            .WithMany(p => p.ProfileLinks)
            .HasForeignKey(d => d.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}