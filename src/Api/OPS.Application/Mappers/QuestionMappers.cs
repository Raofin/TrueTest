using OPS.Application.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Mappers;

public static class QuestionExtensions
{
    public static ProblemQuestionResponse MapToProblemQuestionDto(this Question question)
    {
        return new ProblemQuestionResponse(
            question.Id,
            question.ExaminationId,
            (QuestionType)question.QuestionTypeId,
            question.StatementMarkdown,
            question.Points,
            (DifficultyType)question.DifficultyId,
            question.TestCases.Select(
                tc => new TestCaseResponse(tc.Id, tc.Input, tc.ExpectedOutput)
            ).ToList()
        );
    }

    public static WrittenQuestionResponse MapToWrittenQuestionDto(this Question question)
    {
        return new WrittenQuestionResponse(
            question.Id,
            question.ExaminationId,
            (QuestionType)question.QuestionTypeId,
            question.HasLongAnswer,
            question.StatementMarkdown,
            question.Points,
            (DifficultyType)question.DifficultyId
        );
    }

    public static McqQuestionResponse MapToMcqQuestionDto(this Question question)
    {
        return new McqQuestionResponse(
            question.Id,
            question.ExaminationId,
            (QuestionType)question.QuestionTypeId,
            question.StatementMarkdown,
            question.Points,
            (DifficultyType)question.DifficultyId,
            new McqOptionResponse(
                question.McqOption!.Option1,
                question.McqOption.Option2,
                question.McqOption.Option3,
                question.McqOption.Option4,
                question.McqOption.IsMultiSelect,
                question.McqOption.AnswerOptions
            )
        );
    }
}