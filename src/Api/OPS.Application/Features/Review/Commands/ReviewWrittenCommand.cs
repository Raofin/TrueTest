using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Review.Commands;

public record ReviewWrittenCommand(
    Guid ExamId,
    Guid AccountId,
    Guid WrittenSubmissionId,
    decimal? Score,
    bool? IsFlagged,
    string? FlagReason) : IRequest<ErrorOr<Success>>;

public class ReviewWrittenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewWrittenCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(ReviewWrittenCommand request, CancellationToken cancellationToken)
    {
        var submission = await _unitOfWork.WrittenSubmission.GetAsync(request.WrittenSubmissionId, cancellationToken);
        if (submission is null) return Error.NotFound();

        var candidate = await _unitOfWork.ExamCandidate.GetAsync(request.AccountId, request.ExamId, cancellationToken);
        if (candidate is null) return Error.Unexpected();

        if (request.Score != null)
        {
            candidate.WrittenScore -= submission.Score;
            candidate.WrittenScore += request.Score.Value;
            submission.Score = request.Score.Value;
        }

        submission.IsFlagged = request.IsFlagged ?? submission.IsFlagged;
        submission.FlagReason = request.FlagReason ?? submission.FlagReason;

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class ReviewWrittenCommandValidator : AbstractValidator<ReviewWrittenCommand>
{
    public ReviewWrittenCommandValidator()
    {
        RuleFor(x => x.WrittenSubmissionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100)
            .When(x => x.Score.HasValue);

        RuleFor(x => x.FlagReason)
            .MaximumLength(500)
            .When(x => x.FlagReason is not null);
    }
}