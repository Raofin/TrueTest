using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record ProblemQuestionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Points,
    Guid ExaminationId,
    DifficultyType DifficultyType,
    QuestionType QuestionType,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    List<TestCaseResponse> TestCases
);

public record TestCaseResponse(
    Guid TestCaseId,
    string Input,
    string Output
);

public record TestCaseUpdateRequest(
    Guid? TestCaseId,
    string? Input,
    string? Output
);

public record TestCaseRequest(
    string Input,
    string Output
);

public record McqQuestionResponse(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyType,
    QuestionType QuestionType,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    McqOptionResponse McqOption
);

public record McqOptionResponse(
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool IsMultiSelect,
    string AnswerOptions
);

public record CreateMcqOptionRequest(
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool IsMultiSelect,
    string AnswerOptions
);

public record UpdateMcqOptionRequest(
    string? Option1,
    string? Option2,
    string? Option3,
    string? Option4,
    bool? IsMultiSelect,
    string? AnswerOptions
);

public record WrittenQuestionResponse(
    Guid QuestionId,
    bool HasLongAnswer,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyType,
    QuestionType QuestionType,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record WrittenQuestionUpdateRequest(
    Guid? QuestionId,
    bool? HasLongAnswer,
    string? StatementMarkdown,
    decimal? Score,
    DifficultyType DifficultyType
);