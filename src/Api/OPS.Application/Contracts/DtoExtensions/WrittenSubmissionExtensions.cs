using OPS.Application.Contracts.Submit;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Contracts.DtoExtensions;

public static class WrittenSubmissionExtensions
{
    public static WrittenSubmissionResponse ToDto(this WrittenSubmission writtenSubmission)
    {
        return new WrittenSubmissionResponse(
            writtenSubmission.Id,
            writtenSubmission.QuestionId,
            writtenSubmission.AccountId,
            writtenSubmission.Answer,
            writtenSubmission.Score
        );
    }
}