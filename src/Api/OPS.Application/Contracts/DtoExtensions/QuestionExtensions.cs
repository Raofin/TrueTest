using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;

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
}