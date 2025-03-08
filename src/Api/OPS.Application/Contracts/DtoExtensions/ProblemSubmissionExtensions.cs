using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Contracts.DtoExtensions;

public static class ProblemSubmissionExtensions
{
    public static ProblemSubmissionResponse ToDto(this ProblemSubmission ProblemSubmission)
    {
        return new ProblemSubmissionResponse(
            ProblemSubmission.Id,
            ProblemSubmission.Code,
            ProblemSubmission.Attempts,
            ProblemSubmission.Score,
            ProblemSubmission.IsFlagged,
            ProblemSubmission.FlagReason,
            ProblemSubmission.ProgLanguageId,
            ProblemSubmission.AccountId,
            ProblemSubmission.QuestionId
        );
    }
}