namespace OPS.Application.Contracts.Dtos;

public record ExamResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);

public record ExamWithQuestionsResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    List<ProblemQuestionResponse> ProblemQuestions,
    List<WrittenQuestionResponse> WrittenQuestions,
    List<McqQuestionResponse> McqQuestions
);