using OPS.Application.Dtos;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Mappers;

public static class ExamExtensions
{
    public static ExamResponse MapToDto(this Examination exam)
    {
        return new ExamResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.DurationMinutes,
            exam.Status(),
            exam.OpensAt,
            exam.ClosesAt
        );
    }

    public static string Status(this Examination exam)
    {
        var now = DateTime.UtcNow;

        return now switch
        {
            _ when now < exam.OpensAt => "Scheduled",
            _ when now > exam.ClosesAt => "Ended",
            _ => "Running"
        };
    }
}