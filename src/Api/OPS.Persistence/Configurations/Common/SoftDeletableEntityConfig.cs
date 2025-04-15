using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Common;

namespace OPS.Persistence.Configurations.Common;

public class SoftDeletableEntityConfig<T> : IEntityTypeConfiguration<T> where T : SoftDeletableEntity
{
    public void Configure(EntityTypeBuilder<T> entity)
    {
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        entity.Property(e => e.DeletedAt).HasDefaultValueSql(null).HasColumnType("DateTime");

        entity.HasQueryFilter(e => !e.IsDeleted);
        entity.HasIndex(e => e.IsDeleted).HasFilter("[IsDeleted] = 0");
        entity.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");
    }
}