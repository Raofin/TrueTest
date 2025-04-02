using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record GetResultByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamResultResponse>>;

public class GetResultsByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetResultByCandidateQuery, ErrorOr<ExamResultResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResultResponse>> Handle(GetResultByCandidateQuery request,
        CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, request.AccountId, cancellationToken);

        if (exam is null) return Error.NotFound("Exam not found.");

        var candidate = exam.ExamCandidates.First();
        var account = candidate.Account!;

        var problemSubmissions = account.ProblemSubmissions;
        var writtenSubmissions = account.WrittenSubmissions;
        var mcqSubmissions = account.McqSubmissions;

        var problemSolvingScore = problemSubmissions.Sum(ps => ps.Score);
        var writtenScore = writtenSubmissions.Sum(ws => ws.Score);
        var mcqScore = mcqSubmissions.Sum(ms => ms.Score);

        var problemSolvingPoints = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .Sum(q => q.Points);
        var writtenPoints = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
            .Sum(q => q.Points);
        var mcqPoints = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
            .Sum(q => q.Points);

        var totalScore = problemSolvingScore + writtenScore + mcqScore;
        var totalPoints = problemSolvingPoints + writtenPoints + mcqPoints;

        var isReviewed = problemSubmissions.All(ps => ps.Score.HasValue) &&
                         writtenSubmissions.All(ws => ws.Score.HasValue) &&
                         mcqSubmissions.All(ms => ms.Score.HasValue);

        var hasCheated = problemSubmissions.Any(ps => ps.IsFlagged) ||
                         writtenSubmissions.Any(ws => ws.IsFlagged);

        var result = new CandidateResultResponse(
            isReviewed,
            candidate.StartedAt!.Value,
            candidate.SubmittedAt!.Value,
            totalPoints,
            totalScore,
            problemSolvingPoints,
            problemSolvingScore,
            writtenPoints,
            writtenScore,
            mcqPoints,
            mcqScore,
            hasCheated
        );

        var examSubmission = new SubmissionResponse(
            problemSubmissions.Select(ToSubmissionDto).ToList(),
            writtenSubmissions.Select(
                ws => new WrittenSubmissionResponse(ws.QuestionId, ws.Id, ws.Answer, ws.Score)
            ).ToList(),
            mcqSubmissions.Select(
                ms => new McqSubmissionResponse(ms.QuestionId, ms.Id, ms.AnswerOptions, ms.Score)
            ).ToList()
        );

        return new ExamResultResponse(
            exam.Id,
            exam.Title,
            account.MapToBasicInfoDto(),
            result,
            examSubmission
        );
    }

    private static ProblemSubmissionResponse ToSubmissionDto(ProblemSubmission submission)
    {
        return new ProblemSubmissionResponse(
            submission.QuestionId,
            submission.Id,
            submission.Code,
            submission.Attempts,
            submission.Score,
            submission.IsFlagged,
            submission.FlagReason,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(
                to => new TestCaseOutputResponse(to.TestCaseId, to.IsAccepted, to.ReceivedOutput)
            ).ToList()
        );
    }
}