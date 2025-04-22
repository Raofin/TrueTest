using OPS.Application.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Mappers;

public static class ExamMappers
{
    public static ExamResponse MapToDto(this Examination exam)
    {
        return new ExamResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.TotalPoints,
            exam.ProblemSolvingPoints,
            exam.WrittenPoints,
            exam.McqPoints,
            exam.DurationMinutes,
            exam.IsPublished,
            exam.Status(),
            exam.OpensAt,
            exam.ClosesAt
        );
    }

    public static QuestionResponses MapToQuestionDto(this Examination exam)
    {
        return new QuestionResponses(
            exam.Questions
                .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
                .Select(q => q.MapToProblemQuestionDto())
                .ToList(),
            exam.Questions
                .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
                .Select(q => q.MapToWrittenQuestionDto())
                .ToList(),
            exam.Questions
                .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
                .Select(q => q.MapToMcqQuestionDto())
                .ToList()
        );
    }

    public static ResultResponse? MapToResultDto(this ExamCandidate candidate)
    {
        return candidate.StartedAt is null
            ? null
            : new ResultResponse(
                candidate.ProblemSolvingScore + candidate.WrittenScore + candidate.McqScore,
                candidate.ProblemSolvingScore,
                candidate.WrittenScore,
                candidate.McqScore,
                candidate.StartedAt,
                candidate.SubmittedAt,
                candidate.IsReviewed,
                candidate.HasCheated
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