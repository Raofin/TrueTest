namespace OPS.Application.Contracts.Submit;

public record WrittenSubmissionResponse(
    Guid Id,
    Guid QuestionId,
    Guid AccountId, 
    string Answer,
    decimal Score
);