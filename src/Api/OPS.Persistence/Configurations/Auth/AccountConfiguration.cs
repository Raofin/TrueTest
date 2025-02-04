using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Configurations.Auth;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.ToTable("Accounts", "Auth");
        entity.HasKey(e => e.AccountId);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasIndex(e => e.Username).IsUnique();
        entity.HasIndex(e => e.CloudFileId);

        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PasswordHash).IsRequired();
        entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.HasOne(d => d.CloudFile)
            .WithMany(p => p.Accounts)
            .HasForeignKey(d => d.CloudFileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}