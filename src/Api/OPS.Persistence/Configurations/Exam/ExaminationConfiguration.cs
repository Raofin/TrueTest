﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Configurations.Common;
using static OPS.Persistence.Configurations.Common.Constants;

namespace OPS.Persistence.Configurations.Exam;

public class ExaminationConfiguration : IEntityTypeConfiguration<Examination>
{
    public void Configure(EntityTypeBuilder<Examination> entity)
    {
        entity.ToTable("Examinations", "Exam");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.DescriptionMarkdown).IsRequired();
        entity.Property(e => e.TotalPoints).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.ProblemSolvingPoints).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.WrittenPoints).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.McqPoints).HasColumnType(DecimalType).HasDefaultValueSql("((0))");
        entity.Property(e => e.DurationMinutes).IsRequired();
        entity.Property(e => e.IsPublished).HasDefaultValue(false);
        entity.Property(e => e.OpensAt).HasColumnType("DateTime");
        entity.Property(e => e.ClosesAt).HasColumnType("DateTime");

        new BaseEntityConfig<Examination>().Configure(entity);
        new SoftDeletableEntityConfig<Examination>().Configure(entity);
    }
}