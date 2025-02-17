using OPS.Application.Contracts.Exams;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Extensions;

public static class QuestionExtensions
{
    public static QuestionResponse ToDto(this Question question)
    {
        return new QuestionResponse(
            question.Id, 
            question.StatementMarkdown,
            question.Score,
            question.ExaminationId, 
            question.DifficultyId,    
            question.QuestionTypeId,  
            question.CreatedAt,
            question.UpdatedAt,
            question.IsActive,
            question.IsDeleted
        );
    }
}