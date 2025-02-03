using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Configurations.Submit;

public class McqSubmissionConfiguration : IEntityTypeConfiguration<McqSubmission>
{
    public void Configure(EntityTypeBuilder<McqSubmission> entity)
    {
        entity.ToTable("McqSubmissions", "Submit");
        entity.HasKey(e => e.McqSubmissionId);

        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.Account)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Question)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.McqOption)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.McqOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}