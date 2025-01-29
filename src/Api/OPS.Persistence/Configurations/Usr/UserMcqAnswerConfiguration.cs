using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence.Configurations.Usr;

public partial class UserMcqAnswerConfiguration : IEntityTypeConfiguration<UserMcqAnswer>
{
    public void Configure(EntityTypeBuilder<UserMcqAnswer> entity)
    {
        // Table
        entity.ToTable("UserMcqAnswers", "usr");
        entity.HasKey(e => e.UserMcqAnswerId);

        // Properties
        entity.HasOne(d => d.McqOption).WithMany(p => p.UserMcqAnswers)
            .HasForeignKey(d => d.McqOptionId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        entity.HasOne(d => d.Question).WithMany(p => p.UserMcqAnswers)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        entity.HasOne(d => d.User).WithMany(p => p.UserMcqAnswers)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
