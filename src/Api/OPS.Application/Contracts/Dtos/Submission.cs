namespace OPS.Application.Contracts.Dtos;

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