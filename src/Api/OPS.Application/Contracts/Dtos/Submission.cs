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
    List<TestCaseInputOutputResponse> TestCaseOutputs
);

public record ProblemSubmisionResponse(
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
    ProgLanguageType Language,
    List<TestCaseOutputResponse> TestCaseOutputs
);

public record TestCaseInputOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string Input,
    string ExpectedOutput,
    string ReceivedOutput
);

public record TestCaseOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string ReceivedOutput
);

public record ProblemQuesWithSubmissionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    ProblemSubmissionResponse? ProblemSubmission
);

public record ProblemQuesWithSubmitResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    ProblemSubmitResponse? ProblemSubmit
);

public record McqSubmitResponse(
    Guid QuestionId,
    Guid McqSubmissionId,
    string AnswerOptions
);

public record McqSubmissionResponse(
    Guid QuestionId,
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

public record McqQuesWithSubmitResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    McqSubmitResponse? McqSubmit
);

public record WrittenSubmitResponse(
    Guid QuestionId,
    Guid WrittenSubmissionId,
    string Answer
);

public record WrittenSubmissionResponse(
    Guid QuestionId,
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

public record WrittenQuesWithSubmitResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    WrittenSubmitResponse? WrittenSubmit
);

public record ExamSubmissionResponse(
    Guid ExamId,
    Guid AccountId,
    SubmissionResponse Submission
);

public record SubmissionResponse(
    List<ProblemSubmisionResponse> Problem,
    List<WrittenSubmissionResponse> Written,
    List<McqSubmissionResponse> Mcq
);

public record TestCaseOutputsResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string ReceivedOutput
);