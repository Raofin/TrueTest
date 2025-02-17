namespace OPS.Application.Contracts.Exams;

public record QuestionResponse(
    Guid Id,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    Guid DifficultyId,
    Guid QuestionTypeId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    bool IsDeleted
);