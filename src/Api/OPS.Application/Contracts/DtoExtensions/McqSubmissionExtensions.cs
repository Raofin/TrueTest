using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Contracts.DtoExtensions;

public static class McqSubmissionExtensions
{
    public static McqSubmissionResponse ToDto(this McqSubmission submission)
    {
        return new McqSubmissionResponse(
            submission.Id,
            submission.AnswerOptions,
            submission.QuestionId
        );
    }
}