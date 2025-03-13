using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Contracts.DtoExtensions;

public static class ExamExtensions
{
    public static ExamResponse ToDto(this Examination exam)
    {
        var now = DateTime.UtcNow;
        
        var status = 
            now < exam.OpensAt ? "Scheduled" :
            now > exam.ClosesAt ? "Ended" : "Running";

        return new ExamResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.DurationMinutes,
            status,
            exam.OpensAt,
            exam.ClosesAt,
            exam.CreatedAt,
            exam.UpdatedAt,
            exam.IsActive
        );
    }
}