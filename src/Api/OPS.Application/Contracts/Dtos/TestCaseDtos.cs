namespace OPS.Application.Contracts.Dtos;

public record TestCaseResponse(
    Guid Id,
    Guid QuestionId,
    string Input,
    string Output
);