using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities;

namespace OPS.Persistence.Configurations;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        // Table
        entity.ToTable("Users", "auth");
        entity.HasKey(e => e.UserId);

        // Properties
        entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PasswordHash).IsRequired();
        entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        // Indexes
        entity.HasIndex(e => e.Username).IsUnique();
        entity.HasIndex(e => e.Email).IsUnique();
    }
}
