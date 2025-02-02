using OPS.Application.Contracts.Exams;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Extensions;

public static class ExamExtensions
{
    public static ExamResponse ToDto(this Examination exam)
    {
        return new ExamResponse(
            exam.ExamId,
            exam.Title,
            exam.Description,
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