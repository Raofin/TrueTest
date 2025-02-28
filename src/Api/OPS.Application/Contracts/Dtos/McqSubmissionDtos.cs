using Microsoft.Identity.Client;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Contracts.Dtos;

public record McqSubmissionResponse(
    Guid Id,
    string AnswerOptions,
    Guid McqOptionId,
    Guid AccountId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);