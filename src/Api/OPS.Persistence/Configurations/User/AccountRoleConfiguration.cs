using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Configurations.User;

public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
{
    public void Configure(EntityTypeBuilder<AccountRole> entity)
    {
        entity.ToTable("AccountRoles", "User");
        entity.HasKey(e => new { e.AccountId, e.RoleId });

        entity.HasOne(d => d.Account)
            .WithMany(p => p.AccountRoles)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Role)
            .WithMany(p => p.AccountRoles)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}