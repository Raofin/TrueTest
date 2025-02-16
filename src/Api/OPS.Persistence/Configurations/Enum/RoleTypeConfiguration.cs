using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.Enum;

public class RoleTypeConfiguration
{
    public void Configure(EntityTypeBuilder<RoleType> entity)
    {
        entity.ToTable("RoleTypes", "Enum");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.RoleName).IsUnique();

        entity.Property(e => e.RoleName).IsRequired().HasMaxLength(255);
    }
}