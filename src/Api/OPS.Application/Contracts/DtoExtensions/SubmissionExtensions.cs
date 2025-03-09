using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class SubmissionExtensions
{
    public static ProblemSubmitResponse? ToProblemSubmissionDto(
        this ProblemSubmission? submission, List<TestCase> testCases)
    {
        return submission is null
            ? null
            : new ProblemSubmitResponse(
                submission.Id,
                submission.Code,
                submission.Attempts,
                submission.Score,
                submission.IsFlagged,
                submission.FlagReason,
                (ProgLanguageType)submission.ProgLanguageId,
                submission.TestCaseOutputs
                    .Zip(testCases, (output, testCase) => new TestCaseOutputResponse(
                        output.TestCaseId,
                        output.IsAccepted,
                        testCase.Input,
                        testCase.ExpectedOutput,
                        output.ReceivedOutput
                    )).ToList()
            );
    }

    public static ProblemQuesWithSubmissionResponse ToQuesProblemSubmissionDto(this Question question)
    {
        return new ProblemQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            (DifficultyType)question.DifficultyId,
            question.ProblemSubmissions.FirstOrDefault()?
                .ToProblemSubmissionDto(question.TestCases.ToList())
        );
    }
}