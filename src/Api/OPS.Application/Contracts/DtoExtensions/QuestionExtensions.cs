using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class QuestionExtensions
{
    public static QuestionResponse ToDto(this Question question)
    {
        return new QuestionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            question.ExaminationId,
            question.DifficultyId,
            question.QuestionTypeId,
            question.CreatedAt,
            question.UpdatedAt,
            question.IsActive
        );
    }
    
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
            testCase.Output
        );
    }
}