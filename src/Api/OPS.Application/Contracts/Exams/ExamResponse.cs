namespace OPS.Application.Contracts.Exams;

public record ExamResponse(
    long ExamId,
    string Title,
    string Description,
    DateTime OpensAt,
    DateTime ClosesAt,
    int Duration,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    bool IsDeleted
);