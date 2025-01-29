using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence.Configurations.Usr;

public partial class UserSolutionConfiguration : IEntityTypeConfiguration<UserSolution>
{
    public void Configure(EntityTypeBuilder<UserSolution> entity)
    {
        // Table
        entity.ToTable("UserSolutions", "usr");
        entity.HasKey(e => e.UserSolutionId);

        // Properties
        entity.Property(e => e.Code).IsRequired();
        entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        // Relationships
        entity.HasOne(d => d.Problem).WithMany(p => p.UserSolutions)
            .HasForeignKey(d => d.ProblemId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        entity.HasOne(d => d.ProgLanguages).WithMany(p => p.UserSolutions)
            .HasForeignKey(d => d.ProgLanguagesId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        entity.HasOne(d => d.User).WithMany(p => p.UserSolutions)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}