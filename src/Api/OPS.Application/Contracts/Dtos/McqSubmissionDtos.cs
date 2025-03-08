namespace OPS.Application.Contracts.Dtos;

public record McqSubmissionResponse(
    Guid Id,
    string AnswerOptions,
    Guid McqOptionId,
    Guid AccountId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);