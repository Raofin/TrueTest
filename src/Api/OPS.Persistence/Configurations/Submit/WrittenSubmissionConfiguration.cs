using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Configurations.Submit;

public class WrittenSubmissionConfiguration : IEntityTypeConfiguration<WrittenSubmission>
{
    public void Configure(EntityTypeBuilder<WrittenSubmission> entity)
    {
        entity.ToTable("WrittenSubmissions", "Submit");
        entity.HasKey(e => e.WrittenSubmissionId);

        entity.Property(e => e.Answer).IsRequired();
        entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");

        entity.HasOne(d => d.Question)
            .WithMany(p => p.WrittenSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}