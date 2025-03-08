using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Contracts.DtoExtensions;

public static class McqSubmissionExtensions
{
    public static McqSubmissionResponse ToDto(this McqSubmission mcqSubmission)
    {
        return new McqSubmissionResponse(
            mcqSubmission.Id,
            mcqSubmission.AnswerOptions,
            mcqSubmission.McqOptionId,
            mcqSubmission.AccountId,
            mcqSubmission.CreatedAt,
            mcqSubmission.UpdatedAt
            );
    }
}