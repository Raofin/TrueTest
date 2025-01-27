using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities;

namespace OPS.Persistence.Configurations;

public partial class UserDetailConfiguration : IEntityTypeConfiguration<UserDetail>
{
    public void Configure(EntityTypeBuilder<UserDetail> entity)
    {
        // Table
        entity.ToTable("UserDetails", "core");
        entity.HasKey(e => e.UserDetailsId);
        entity.HasIndex(e => e.UserId).IsUnique();

        // Properties
        entity.Property(e => e.FirstName).IsRequired().HasMaxLength(255);
        entity.Property(e => e.LastName).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.InstituteName).HasMaxLength(255);

        // Relationships
        entity.HasOne(d => d.User)
            .WithOne(p => p.UserDetail)
            .HasForeignKey<UserDetail>(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
