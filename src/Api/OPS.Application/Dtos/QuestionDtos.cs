using OPS.Domain.Enums;

namespace OPS.Application.Dtos;

public record ProblemQuestionResponse(
    Guid QuestionId,
    Guid ExamId,
    QuestionType QuestionType,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
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
    Guid ExamId,
    QuestionType QuestionType,
    string StatementMarkdown,
    decimal Score,
    DifficultyType DifficultyType,
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
    Guid ExamId,
    QuestionType QuestionType,
    bool HasLongAnswer,
    string StatementMarkdown,
    decimal Score,
    DifficultyType DifficultyType
);

public record WrittenQuestionUpdateRequest(
    Guid? QuestionId,
    bool? HasLongAnswer,
    string? StatementMarkdown,
    decimal? Score,
    DifficultyType DifficultyType
);