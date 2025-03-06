namespace OPS.Application.Contracts.Dtos;

public record WrittenQuestionResponse(
    Guid Id,
    bool HasLongAnswer,   
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive  
);