using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record ProblemSubmitResponse(
    Guid ProblemSubmissionId,
    string Code,
    int Attempts,
    decimal? Score,
    bool IsFlagged,
    string? FlagReason,
    ProgLanguageType ProgLanguageType,
    List<TestCaseOutputResponse> TestCaseOutputs
);

public record TestCaseOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string Input,
    string ExpectedOutput,
    string ReceivedOutput
);

public record ProblemQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    ProblemSubmitResponse? ProblemSubmission
);