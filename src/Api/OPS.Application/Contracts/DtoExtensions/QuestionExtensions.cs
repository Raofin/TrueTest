using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class QuestionExtensions
{
    public static ProblemQuestionResponse ToProblemQuestionDto(this Question question)
    {
        return new ProblemQuestionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            question.ExaminationId,
            (DifficultyType)question.DifficultyId,
            (QuestionType)question.QuestionTypeId,
            question.CreatedAt,
            question.UpdatedAt,
            question.IsActive,
            question.TestCases.Select(tc => tc.ToDto()).ToList()
        );
    }

    public static TestCaseResponse ToDto(this TestCase testCase)
    {
        return new TestCaseResponse(
            testCase.Id,
            testCase.Input,
            testCase.ExpectedOutput
        );
    }

    public static McqQuestionResponse ToMcqQuestionDto(this Question question)
    {
        return new McqQuestionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            question.ExaminationId,
            (DifficultyType)question.DifficultyId,
            (QuestionType)question.QuestionTypeId,
            question.CreatedAt,
            question.UpdatedAt,
            question.IsActive,
            question.McqOption!.ToDto()
        );
    }

    public static McqOptionResponse ToDto(this McqOption mcqOption)
    {
        return new McqOptionResponse(
            mcqOption.Option1,
            mcqOption.Option2,
            mcqOption.Option3,
            mcqOption.Option4,
            mcqOption.IsMultiSelect,
            mcqOption.AnswerOptions
        );
    }

    public static WrittenQuestionResponse ToWrittenQuestionDto(this Question question)
    {
        return new WrittenQuestionResponse(
            question.Id,
            question.HasLongAnswer,
            question.StatementMarkdown,
            question.Points,
            question.ExaminationId,
            (DifficultyType)question.DifficultyId,
            (QuestionType)question.QuestionTypeId,
            question.CreatedAt,
            question.UpdatedAt
        );
    }
}