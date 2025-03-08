namespace OPS.Application.Contracts.Dtos;

public record ProblemSubmissionResponse(
    Guid Id,
    string Code,
    int Attempts,
    decimal? Score,
    bool IsFlagged,
    string? FlagReason,
    int ProgLanguageId,
    Guid AccountId,
    Guid QuestionId
);