namespace OPS.Application.Contracts.Dtos;

public record McqOptionResponse(
    Guid Id,
    Guid QuestionId,
    string Option1,
    string Option2,
    string Option3,
    string Option4,
    bool isMultiSelect,
    string AnswerOptions,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);