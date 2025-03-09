using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class SubmissionExtensions
{
    public static ProblemSubmitResponse ToProblemSubmitDto(
        this ProblemSubmission submission, List<TestCase> testCases)
    {
        return new ProblemSubmitResponse(
            submission.Id,
            submission.Code,
            submission.Attempts,
            submission.Score,
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
}