using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Configurations.Auth;

public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
{
    public void Configure(EntityTypeBuilder<AccountRole> entity)
    {
        entity.ToTable("AccountRoles", "Auth");
        entity.HasKey(e => new { e.AccountId, e.RoleTypeId });

        entity.HasOne(d => d.Account)
            .WithMany(p => p.AccountRoles)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.RoleType)
            .WithMany(p => p.AccountRoles)
            .HasForeignKey(d => d.RoleTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}