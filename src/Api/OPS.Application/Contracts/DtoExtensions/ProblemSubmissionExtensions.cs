using Microsoft.Identity.Client;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
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