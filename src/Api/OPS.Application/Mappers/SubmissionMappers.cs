using OPS.Application.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Mappers;

public static class SubmissionExtensions
{
    public static ProblemQuesWithSubmissionResponse? ToProblemWithSubmissionDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.ProblemSolving)
            return null;

        var submission = question.ProblemSubmissions.FirstOrDefault();
        var testCases = question.TestCases.ToList();

        return new ProblemQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            (DifficultyType)question.DifficultyId,
            submission is null
                ? null
                : new ProblemSubmissionTcResponse(
                    submission.Id,
                    submission.Code,
                    submission.Attempts,
                    submission.Score,
                    submission.IsFlagged,
                    submission.FlagReason,
                    (ProgLanguageType)submission.ProgLanguageId,
                    MapTestCaseOutputs(submission.TestCaseOutputs, testCases)
                )
        );
    }

    private static List<TestCaseInputOutputResponse> MapTestCaseOutputs(
        IEnumerable<TestCaseOutput> outputs, List<TestCase> testCases)
    {
        return outputs.Zip(testCases, (output, testCase) => new TestCaseInputOutputResponse(
            output.TestCaseId,
            output.IsAccepted,
            testCase.Input,
            testCase.ExpectedOutput,
            output.ReceivedOutput
        )).ToList();
    }

    public static McqQuesWithSubmissionResponse? ToMcqWithSubmissionDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.MCQ)
            return null;

        var submission = question.McqSubmissions.FirstOrDefault();
        var submissionResponse = submission is null
            ? null
            : new McqSubmissionResponse(
                submission.QuestionId,
                submission.Id,
                submission.AnswerOptions,
                submission.Score
            );

        return new McqQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            submissionResponse
        );
    }

    public static WrittenQuesWithSubmissionResponse? ToWrittenWithSubmissionDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.Written)
            return null;

        var submission = question.WrittenSubmissions.FirstOrDefault();
        var submissionResponse = submission is null
            ? null
            : new WrittenSubmissionResponse(
                submission.QuestionId,
                submission.Id,
                submission.Answer,
                submission.Score
            );

        return new WrittenQuesWithSubmissionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Points,
            submissionResponse
        );
    }
}