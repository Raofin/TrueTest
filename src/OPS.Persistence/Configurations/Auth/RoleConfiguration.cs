using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Auth;

public partial class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        // Table
        entity.ToTable("Roles", "enum");
        entity.HasKey(e => e.RoleId);
        entity.HasIndex(e => e.RoleName).IsUnique();

        // Properties
        entity.Property(e => e.RoleName).IsRequired().HasMaxLength(255);
    }
}
