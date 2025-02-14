namespace OPS.Application.Contracts.Dtos;

public record ExamResponse(
    Guid Id,
    string Title,
    string Description,
    int Duration,
    DateTime OpensAt,
    DateTime ClosesAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive
);