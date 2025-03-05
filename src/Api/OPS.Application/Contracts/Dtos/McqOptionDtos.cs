namespace OPS.Application.Contracts.Dtos;

public record McqOptionResponse(
    Guid Id,
    Guid QuestionId,
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool isMultiSelect,
    string AnswerOptions,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateMcqOptionResponse(
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool IsMultiSelect,
    string AnswerOptions
);


public record UpdateMcqOptionResponse(
    Guid Id,
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool IsMultiSelect,
    string AnswerOptions
);