using OPS.Application.Contracts.Exams;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Extensions;

public static class ExamExtensions
{
    public static ProfileResponse ToDto(this Examination exam)
    {
        return new ProfileResponse(
            exam.ExaminationId,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.OpensAt,
            exam.ClosesAt,
            exam.Duration,
            exam.CreatedAt,
            exam.UpdatedAt,
            exam.IsActive,
            exam.IsDeleted
        );
    }
}