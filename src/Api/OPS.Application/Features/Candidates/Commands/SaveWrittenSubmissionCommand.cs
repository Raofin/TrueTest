using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Candidates.Commands;

public record SaveWrittenSubmissionCommand(Guid QuestionId, string CandidateAnswer)
    : IRequest<ErrorOr<WrittenSubmitResponse>>;

public class SaveWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveWrittenSubmissionCommand, ErrorOr<WrittenSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<WrittenSubmitResponse>> Handle(SaveWrittenSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
        if (question == null) return Error.NotFound();

        var existingSubmission = await _unitOfWork.WrittenSubmission.GetByAccountIdAsync(
            request.QuestionId, userAccountId, cancellationToken);

        var submission = new WrittenSubmission
        {
            Answer = request.CandidateAnswer,
            QuestionId = request.QuestionId,
            AccountId = userAccountId
        };

        if (existingSubmission is null)
        {
            _unitOfWork.WrittenSubmission.Add(submission);
        }
        else
        {
            existingSubmission.Answer = request.CandidateAnswer;
            submission = existingSubmission;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return new WrittenSubmitResponse(
            submission.QuestionId,
            submission.Id,
            submission.Answer
        );
    }
}

public class SaveWrittenSubmissionCommandValidator : AbstractValidator<SaveWrittenSubmissionCommand>
{
    public SaveWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.CandidateAnswer).NotEmpty();
    }
}