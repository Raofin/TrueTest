using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> entity)
    {
        entity.ToTable("Profiles", "User");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.AccountId).IsUnique();

        entity.Property(e => e.FirstName).HasMaxLength(255);
        entity.Property(e => e.LastName).HasMaxLength(255);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.InstituteName).HasMaxLength(255);

        new BaseEntityConfig<Profile>().Configure(entity);
        new SoftDeletableEntityConfig<Profile>().Configure(entity);

        entity.HasOne(d => d.Account)
            .WithOne(p => p.Profile)
            .HasForeignKey<Profile>(d => d.AccountId);

        entity.HasOne(d => d.ImageFile)
            .WithOne(p => p.Profile)
            .HasForeignKey<Profile>(d => d.ImageFileId);
    }
}