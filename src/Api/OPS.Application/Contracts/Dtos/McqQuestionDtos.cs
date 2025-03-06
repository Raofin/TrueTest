using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record McqQuestionResponse(
    Guid Id,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    List<McqOptionResponse> McqOptions
);

public record CreateMcqQuestionResponse(
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyId,
    QuestionType QuestionTypeId,
    List<CreateMcqOptionResponse> McqOptions
);

public record UpdateMcqQuestionResponse(
    Guid Id,    
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyId,
    QuestionType QuestionTypeId,
    List<UpdateMcqOptionResponse> McqOptions
);