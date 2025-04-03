using OPS.Domain.Enums;

namespace OPS.Application.Dtos;

public record ProblemQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    ProblemSubmissionTcResponse? ProblemSubmission
);

public record ProblemSubmissionTcResponse(
    Guid ProblemSubmissionId,
    string Code,
    int Attempts,
    decimal? Score,
    bool IsFlagged,
    string? FlagReason,
    ProgLanguageType Language,
    List<TestCaseInputOutputResponse> TestCaseOutputs
);

public record TestCaseInputOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string Input,
    string ExpectedOutput,
    string ReceivedOutput
);

public record ProblemSubmissionResponse(
    Guid QuestionId,
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
    Guid QuestionId,
    Guid ProblemSubmissionId,
    string Code,
    ProgLanguageType Language
);

public record TestCaseOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string ReceivedOutput
);

public record McqQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    McqSubmissionResponse? McqSubmission
);

public record McqSubmissionResponse(
    Guid QuestionId,
    Guid McqSubmissionId,
    string AnswerOptions,
    decimal? Score
);

public record McqSubmitResponse(
    Guid QuestionId,
    Guid McqSubmissionId,
    string AnswerOptions
);

public record WrittenQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    WrittenSubmissionResponse? WrittenSubmission
);

public record WrittenSubmissionResponse(
    Guid QuestionId,
    Guid WrittenSubmissionId,
    string Answer,
    decimal? Score
);

public record WrittenSubmitResponse(
    Guid QuestionId,
    Guid WrittenSubmissionId,
    string Answer
);

public record SubmissionResponse(
    List<ProblemSubmissionResponse?> Problem,
    List<WrittenSubmissionResponse?> Written,
    List<McqSubmissionResponse?> Mcq
);

public record SubmitResponse(
    List<ProblemSubmitResponse?> Problem,
    List<WrittenSubmitResponse?> Written,
    List<McqSubmitResponse?> Mcq
);