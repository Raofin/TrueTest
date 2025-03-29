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
            submission.QuestionId,
            submission.Id,
            submission.Code,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(tco => new TestCaseOutputResponse(
                tco.TestCaseId,
                tco.IsAccepted,
                tco.ReceivedOutput
            )).ToList()
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

    public static ProblemSubmisionResponse ToSubmissionDto(this ProblemSubmission submission)
    {
        return new ProblemSubmisionResponse(
            submission.QuestionId,
            submission.Id,
            submission.Code,
            submission.Attempts,
            submission.Score,
            submission.IsFlagged,
            submission.FlagReason,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(
                output => new TestCaseOutputResponse(
                    output.TestCaseId,
                    output.IsAccepted,
                    output.ReceivedOutput
                )).ToList()
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

    public static ProblemQuesWithSubmissionResponse? ToProblemWithSubmissionDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.ProblemSolving
            ? null
            : new ProblemQuesWithSubmissionResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                (DifficultyType)question.DifficultyId,
                question.ProblemSubmissions.FirstOrDefault()?
                    .ToProblemSubmissionDto(question.TestCases.ToList())
            );
    }

    public static ProblemSubmitResponse? ToProblemSubmitDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.ProblemSolving
            || question.ProblemSubmissions.FirstOrDefault() is not null)
            return null;

        var submission = question.ProblemSubmissions.First();

        return new ProblemSubmitResponse(
            question.Id,
            submission.Id,
            submission.Code,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(tco => new TestCaseOutputResponse(
                tco.TestCaseId,
                tco.IsAccepted,
                tco.ReceivedOutput
            )).ToList()
        );
    }

    public static WrittenSubmitResponse? ToWrittenSubmitDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.Written ||
            question.WrittenSubmissions.FirstOrDefault() is not null)
            return null;

        var submission = question.WrittenSubmissions.First();

        return new WrittenSubmitResponse(
            question.Id,
            submission.Id,
            submission.Answer
        );
    }

    public static McqSubmitResponse? ToMcqSubmitDto(this Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.MCQ ||
            question.McqSubmissions.FirstOrDefault() is not null)
            return null;

        var submission = question.McqSubmissions.First();

        return new McqSubmitResponse(
            question.Id,
            submission.Id,
            submission.AnswerOptions
        );
    }

    public static ProblemQuesWithSubmitResponse? ToProblemWithSubmitDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.ProblemSolving
            ? null
            : new ProblemQuesWithSubmitResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                (DifficultyType)question.DifficultyId,
                question.ProblemSubmissions.FirstOrDefault()?
                    .ToProblemSubmitDto(question.TestCases.ToList())
            );
    }

    public static McqSubmitResponse ToSubmitDto(this McqSubmission submission)
    {
        return new McqSubmitResponse(
            submission.QuestionId,
            submission.Id,
            submission.AnswerOptions
        );
    }

    public static McqQuesWithSubmissionResponse? ToMcqWithSubmissionDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.MCQ
            ? null
            : new McqQuesWithSubmissionResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                question.McqSubmissions.FirstOrDefault() is { } submission
                    ? new McqSubmissionResponse(
                        submission.QuestionId,
                        submission.Id,
                        submission.AnswerOptions,
                        submission.Score)
                    : null
            );
    }

    public static McqSubmissionResponse ToSubmissionDto(this McqSubmission submission)
    {
        return new McqSubmissionResponse(
            submission.QuestionId,
            submission.Id,
            submission.AnswerOptions,
            submission.Score
        );
    }

    public static McqQuesWithSubmitResponse? ToMcqWithSubmitDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.MCQ
            ? null
            : new McqQuesWithSubmitResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                question.McqSubmissions.FirstOrDefault() is { } submission
                    ? new McqSubmitResponse(question.Id, submission.Id, submission.AnswerOptions)
                    : null
            );
    }

    public static WrittenSubmitResponse ToSubmitDto(this WrittenSubmission submission)
    {
        return new WrittenSubmitResponse(
            submission.QuestionId,
            submission.Id,
            submission.Answer
        );
    }

    public static WrittenQuesWithSubmissionResponse? ToWrittenWithSubmissionDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.Written
            ? null
            : new WrittenQuesWithSubmissionResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                question.WrittenSubmissions.FirstOrDefault() is { } submission
                    ? new WrittenSubmissionResponse(
                        submission.QuestionId,
                        submission.Id,
                        submission.Answer,
                        submission.Score)
                    : null
            );
    }

    public static WrittenSubmissionResponse ToSubmissionDto(this WrittenSubmission submission)
    {
        return new WrittenSubmissionResponse(
            submission.QuestionId,
            submission.Id,
            submission.Answer,
            submission.Score
        );
    }

    public static WrittenQuesWithSubmitResponse? ToWrittenWithSubmitDto(this Question question)
    {
        return question.QuestionTypeId != (int)QuestionType.Written
            ? null
            : new WrittenQuesWithSubmitResponse(
                question.Id,
                question.StatementMarkdown,
                question.Points,
                question.WrittenSubmissions.FirstOrDefault() is { } submission
                    ? new WrittenSubmitResponse(question.Id, submission.Id, submission.Answer)
                    : null
            );
    }
}