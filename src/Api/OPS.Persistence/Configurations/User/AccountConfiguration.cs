using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.ToTable("Accounts", "User");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasIndex(e => e.Username).IsUnique();

        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PasswordHash).IsRequired();
        entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Username).IsRequired().HasMaxLength(255);

        new BaseEntityConfig<Account>().Configure(entity);
        new SoftDeletableEntityConfig<Account>().Configure(entity);
    }
}