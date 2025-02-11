using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Common;

namespace OPS.Persistence.Configurations.Common;

public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GetUtcDate()")
            .HasColumnType("DateTime")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("GetUtcDate()")
            .HasColumnType("DateTime")
            .ValueGeneratedOnAddOrUpdate();
    }
}