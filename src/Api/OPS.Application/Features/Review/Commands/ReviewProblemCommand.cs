using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Domain;

namespace OPS.Application.Features.Review.Commands;

public record ReviewProblemCommand(
    Guid ExamId,
    Guid AccountId,
    Guid ProblemSubmissionId,
    decimal? Score,
    bool? IsFlagged,
    string? FlagReason) : IRequest<ErrorOr<Success>>;

public class ReviewProblemCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewProblemCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(ReviewProblemCommand request, CancellationToken cancellationToken)
    {
        var submission = await _unitOfWork.ProblemSubmission.GetAsync(request.ProblemSubmissionId, cancellationToken);
        if (submission is null) return Error.NotFound();

        var candidate = await _unitOfWork.ExamCandidate.GetAsync(request.AccountId, request.ExamId, cancellationToken);
        if (candidate is null) return Error.Unexpected();

        if (request.Score is not null)
        {
            candidate.ProblemSolvingScore -= submission.Score;
            candidate.ProblemSolvingScore += request.Score.Value;
            submission.Score = request.Score.Value;
        }

        submission.IsFlagged = request.IsFlagged ?? submission.IsFlagged;
        submission.FlagReason = request.FlagReason ?? submission.FlagReason;

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class ReviewProblemCommandValidator : AbstractValidator<ReviewProblemCommand>
{
    public ReviewProblemCommandValidator()
    {
        RuleFor(x => x.ProblemSubmissionId)
            .IsValidGuid();

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100)
            .When(x => x.Score.HasValue);

        RuleFor(x => x.FlagReason)
            .MaximumLength(500);
    }
}