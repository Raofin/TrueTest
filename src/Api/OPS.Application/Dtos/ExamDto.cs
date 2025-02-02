namespace OPS.Application.Dtos;

public record ExamDto(
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

public record ExamCreateDto(
    string Title,
    string Description,
    DateTime OpensAt,
    DateTime ClosesAt,
    int Duration,
    bool IsActive
);

public record ExamUpdateDto(
    long ExamId,
    string? Title,
    string? Description,
    DateTime? OpensAt,
    DateTime? ClosesAt,
    int? Duration,
    bool? IsActive,
    bool? IsDeleted
);