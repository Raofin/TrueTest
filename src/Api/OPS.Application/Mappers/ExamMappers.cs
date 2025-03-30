using OPS.Application.Dtos;
using OPS.Domain.Entities.Exam;
using QuestionType = OPS.Domain.Enums.QuestionType;

namespace OPS.Application.Mappers;

public static class ExamExtensions
{
    public static ExamResponse ToDto(this Examination exam)
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

    public static ExamWithQuestionsResponse ToDtoWithQuestions(this Examination exam)
    {
        var problemQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .Select(q => q.ToProblemQuestionDto())
            .ToList();

        var writtenQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
            .Select(q => q.ToWrittenQuestionDto())
            .ToList();

        var mcqQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
            .Select(q => q.ToMcqQuestionDto())
            .ToList();

        return new ExamWithQuestionsResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.DurationMinutes,
            Status(exam),
            exam.OpensAt,
            exam.ClosesAt,
            new QuestionResponses(
                problemQuestions,
                writtenQuestions,
                mcqQuestions
            )
        );
    }

    private static string Status(this Examination exam)
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