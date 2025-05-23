﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Configurations.Common;
using static OPS.Persistence.Configurations.Common.Constants;

namespace OPS.Persistence.Configurations.Submit;

public class WrittenSubmissionConfiguration : IEntityTypeConfiguration<WrittenSubmission>
{
    public void Configure(EntityTypeBuilder<WrittenSubmission> entity)
    {
        entity.ToTable("WrittenSubmissions", "Submit");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Answer).IsRequired();
        entity.Property(e => e.Score).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.IsFlagged).HasDefaultValue(false);

        new BaseEntityConfig<WrittenSubmission>().Configure(entity);

        entity.HasOne(d => d.Question)
            .WithMany(p => p.WrittenSubmissions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}