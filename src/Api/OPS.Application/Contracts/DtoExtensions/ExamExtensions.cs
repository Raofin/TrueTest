using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Contracts.DtoExtensions;

public static class ExamExtensions
{
    public static ExamResponse ToDto(this Examination exam)
    {
        var now = DateTime.UtcNow;

        string status;
        
        if (now < exam.OpensAt)
        {
            status = "Scheduled";
        }
        else if (now > exam.ClosesAt)
        {
            status = "Ended";
        }
        else
        {
            status = "Running";
        }

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