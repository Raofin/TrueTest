using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Configurations.Enum;

public class RoleTypeConfiguration : IEntityTypeConfiguration<RoleType>
{
    public void Configure(EntityTypeBuilder<RoleType> entity)
    {
        entity.ToTable("RoleTypes", "Enum");
        entity.HasKey(e => e.RoleTypeId);
        entity.HasIndex(e => e.RoleName).IsUnique();
        entity.Property(e => e.RoleName).IsRequired().HasMaxLength(255);
    }
}