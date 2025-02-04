using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Configurations.User;

public class AccountSocialConfiguration : IEntityTypeConfiguration<AccountSocial>
{
    public void Configure(EntityTypeBuilder<AccountSocial> entity)
    {
        entity.ToTable("AccountSocials", "User");
        entity.HasKey(e => e.SocialLinkId);

        entity.HasOne(d => d.Account)
            .WithMany(p => p.AccountSocials)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.SocialLink)
            .WithMany(p => p.AccountSocials)
            .HasForeignKey(d => d.SocialLinkId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}