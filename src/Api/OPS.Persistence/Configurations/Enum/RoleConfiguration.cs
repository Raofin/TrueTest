using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.ToTable("Roles", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.RoleName).IsUnique();

        entity.Property(e => e.RoleName).IsRequired().HasMaxLength(255);
    }
}