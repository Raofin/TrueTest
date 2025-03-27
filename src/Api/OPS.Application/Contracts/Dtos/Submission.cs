using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record ProblemSubmissionResponse(
    Guid ProblemSubmissionId,
    string Code,
    int Attempts,
    decimal? Score,
    bool IsFlagged,
    string? FlagReason,
    ProgLanguageType Language,
    List<TestCaseOutputResponse> TestCaseOutputs
);

public record ProblemSubmitResponse(
    Guid ProblemSubmissionId,
    string Code,
    ProgLanguageType Language,
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
    ProblemSubmissionResponse? ProblemSubmission
);

public record McqSubmitResponse(
    Guid McqSubmissionId,
    string AnswerOptions,
    Guid QuestionId
);

public record McqSubmissionResponse(
    Guid McqSubmissionId,
    string AnswerOptions,
    decimal? Score
);

public record McqQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    McqSubmissionResponse? McqSubmission
);

public record WrittenSubmitResponse(
    Guid WrittenSubmissionId,
    string Answer,
    Guid QuestionId
);

public record WrittenSubmissionResponse(
    Guid WrittenSubmissionId,
    string Answer,
    decimal? Score
);

public record WrittenQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    WrittenSubmissionResponse? WrittenSubmission
);