using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence.Configurations.Usr;

public partial class UserWrittenAnswerConfiguration : IEntityTypeConfiguration<UserWrittenAnswer>
{
    public void Configure(EntityTypeBuilder<UserWrittenAnswer> entity)
    {
        // Table
        entity.ToTable("UserWrittenAnswers", "usr");
        entity.HasKey(e => e.UserWrittenAnswerId);

        // Properties
        entity.Property(e => e.Answer).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

        // Relationships
        entity.HasOne(d => d.Question).WithMany(p => p.UserWrittenAnswers)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
