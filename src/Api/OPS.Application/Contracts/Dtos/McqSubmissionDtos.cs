namespace OPS.Application.Contracts.Dtos;

public record McqSubmissionResponse(
    Guid McqSubmissionId,
    string AnswerOptions,
    Guid QuestionId
);