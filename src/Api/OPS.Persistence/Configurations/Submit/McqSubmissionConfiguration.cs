using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Configurations.Common;
using static OPS.Persistence.Configurations.Common.Constants;

namespace OPS.Persistence.Configurations.Submit;

public class McqSubmissionConfiguration : IEntityTypeConfiguration<McqSubmission>
{
    public void Configure(EntityTypeBuilder<McqSubmission> entity)
    {
        entity.ToTable("McqSubmissions", "Submit");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Score).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.AnswerOptions).IsRequired().HasMaxLength(50);

        new BaseEntityConfig<McqSubmission>().Configure(entity);

        entity.HasOne(d => d.Account)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.McqOption)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.McqOptionId)
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(d => d.Question)
            .WithMany(p => p.McqSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}