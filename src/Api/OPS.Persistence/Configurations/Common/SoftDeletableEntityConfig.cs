using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Common;

namespace OPS.Persistence.Configurations.Common;

public class SoftDeletableEntityConfig<T> : IEntityTypeConfiguration<T> where T : SoftDeletableEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.HasIndex(e => e.IsDeleted).HasFilter("[IsDeleted] = 0");
        builder.HasIndex(e => e.IsActive).HasFilter("[IsActive] = 1");
    }
}