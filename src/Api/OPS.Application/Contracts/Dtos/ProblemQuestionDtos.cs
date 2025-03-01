namespace OPS.Application.Contracts.Dtos;

public record ProblemQuestionResponse(
    Guid Id,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    List<TestCaseResponse> TestCases    
);