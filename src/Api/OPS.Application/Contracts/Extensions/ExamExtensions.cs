using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Contracts.Extensions;

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
            exam.IsActive
        );
    }
}