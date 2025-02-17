using OPS.Application.Contracts.Exams;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Extensions;

public static class ExamExtensions
{
    public static ExamResponse ToDto(this Examination exam)
    {
        return new ExamResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.DurationMinutes,
            exam.OpensAt,
            exam.ClosesAt,
            exam.CreatedAt,
            exam.UpdatedAt,
            exam.IsActive,
            exam.IsDeleted
        );
    }
}