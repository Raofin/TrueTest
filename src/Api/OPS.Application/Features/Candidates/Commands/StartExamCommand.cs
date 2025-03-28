using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using Throw;

namespace OPS.Application.Features.Candidates.Commands;

public record StartExamCommand(Guid ExamId) : IRequest<ErrorOr<ExamStartResponse>>;

public class StartExamCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<StartExamCommand, ErrorOr<ExamStartResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<ExamStartResponse>> Handle(
        StartExamCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var candidate = await _unitOfWork.Exam.GetCandidateAsync(
            request.ExamId, userAccountId, cancellationToken);

        if (candidate == null)
        {
            return Error.Unauthorized(description: "Candidate was not invited to this exam");
        }

        if (candidate.SubmittedAt != null || candidate.Examination.ClosesAt > DateTime.UtcNow)
        {
            return Error.Unauthorized(description: "Exam is already submitted or ended");
        }

        if (candidate.StartedAt == null)
        {
            candidate.StartedAt = DateTime.UtcNow;
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, userAccountId, cancellationToken);

        exam.ThrowIfNull();

        var examReview = new ExamStartResponse(
            candidate.StartedAt.Value,
            candidate.StartedAt.Value.AddMinutes(exam.DurationMinutes),
            exam.ToDto(),
            new QuestionsWithSubmitsResposne(
                exam.Questions.Select(q => q.ToProblemWithSubmitDto()).ToList(),
                exam.Questions.Select(q => q.ToWrittenWithSubmitDto()).ToList(),
                exam.Questions.Select(q => q.ToMcqWithSubmitDto()).ToList()
            )
        );

        return examReview;
    }
}

public class StartExamCommandValidator : AbstractValidator<StartExamCommand>
{
    public StartExamCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}