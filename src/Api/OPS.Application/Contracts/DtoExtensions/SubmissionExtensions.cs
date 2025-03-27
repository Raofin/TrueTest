using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class SubmissionExtensions
{
    public static ProblemSubmitResponse? ToProblemSubmitDto(
        this ProblemSubmission? submission, List<TestCase> testCases)
    {
        if (submission is null) return null;

        return new ProblemSubmitResponse(
            submission.Id,
            submission.Code,
            (ProgLanguageType)submission.ProgLanguageId,
            MapTestCaseOutputs(submission.TestCaseOutputs, testCases)
        );
    }

    public static ProblemSubmissionResponse? ToProblemSubmissionDto(
        this ProblemSubmission? submission, List<TestCase> testCases)
    {
        if (submission is null) return null;

        return new ProblemSubmissionResponse(
            submission.Id,
            submission.Code,
            submission.Attempts,
            submission.Score,
            submission.IsFlagged,
            submission.FlagReason,
            (ProgLanguageType)submission.ProgLanguageId,
            MapTestCaseOutputs(submission.TestCaseOutputs, testCases)
        );
    }

    private static List<TestCaseOutputResponse> MapTestCaseOutputs(
        IEnumerable<TestCaseOutput> outputs, List<TestCase> testCases)
    {
        return outputs.Zip(testCases, (output, testCase) => new TestCaseOutputResponse(
            output.TestCaseId,
            output.IsAccepted,
            testCase.Input,
            testCase.ExpectedOutput,
            output.ReceivedOutput
        )).ToList();
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

    public static McqSubmitResponse ToDto(this McqSubmission submission)
    {
        return new McqSubmitResponse(
            submission.Id,
            submission.AnswerOptions,
            submission.QuestionId
        );
    }

    public static McqQuesWithSubmissionResponse ToMcqWithSubmissionDto(this Question question)
    {
        return new McqQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            question.McqSubmissions.FirstOrDefault() is { } submission
                ? new McqSubmissionResponse(submission.Id, submission.AnswerOptions, submission.Score)
                : null
        );
    }

    public static WrittenSubmitResponse ToDto(this WrittenSubmission submission)
    {
        return new WrittenSubmitResponse(
            submission.Id,
            submission.Answer,
            submission.QuestionId
        );
    }

    public static WrittenQuesWithSubmissionResponse ToWrittenWithSubmissionDto(this Question question)
    {
        return new WrittenQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            question.WrittenSubmissions.FirstOrDefault() is { } submission
                ? new WrittenSubmissionResponse(submission.Id, submission.Answer, submission.Score)
                : null
        );
    }
}