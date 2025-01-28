using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence.Configurations.Auth;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        // Table
        entity.ToTable("Users", "auth");
        entity.HasKey(e => e.UserId);
        entity.HasIndex(e => e.Username).IsUnique();
        entity.HasIndex(e => e.Email).IsUnique();

        // Properties
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.PasswordHash).IsRequired();
        entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Username).IsRequired().HasMaxLength(255);

        // Relationships
        entity.HasOne(d => d.CloudFile)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.CloudFileId);
        entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__RoleI__5070F446"),
                l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__UserI__4F7CD00D"),
                j =>
                {
                    j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD02D51B95");
                    j.ToTable("UserRoles", "auth");
                });

        entity.HasMany(d => d.Socials).WithMany(p => p.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserSocial",
                r => r.HasOne<Social>().WithMany()
                        .HasForeignKey("SocialId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserSocia__Socia__49C3F6B7"),
                l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserSocia__UserI__48CFD27E"),
                j =>
                {
                    j.HasKey("UserId", "SocialId").HasName("PK__UserSoci__A1F43B5D0C5D0F96");
                    j.ToTable("UserSocials", "usr");
                });
    }
}
