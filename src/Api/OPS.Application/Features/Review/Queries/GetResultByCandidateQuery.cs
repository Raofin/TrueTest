using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record GetResultByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<CandidateResultResponse>>;

public class GetResultsByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetResultByCandidateQuery, ErrorOr<CandidateResultResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<CandidateResultResponse>> Handle(GetResultByCandidateQuery request,
        CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, request.AccountId, cancellationToken);

        if (exam is null) return Error.NotFound("Exam not found.");

        var account = exam.ExamCandidates.First().Account!;

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

        return new CandidateResultResponse(
            account.Id,
            account.Username,
            account.Email,
            exam.Id,
            exam.DurationMinutes,
            isReviewed,
            exam.ExamCandidates.First().StartedAt!.Value,
            exam.ExamCandidates.First().SubmittedAt,
            hasCheated,
            totalPoints,
            totalScore,
            problemSolvingPoints,
            problemSolvingScore,
            writtenPoints,
            writtenScore,
            mcqPoints,
            mcqScore
        );
    }
}