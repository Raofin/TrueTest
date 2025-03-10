using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Submit;
using Throw;

namespace OPS.Application.Features.Submissions.Commands;

public record SaveWrittenSubmissionCommand(Guid QuestionId, string Answer)
    : IRequest<ErrorOr<WrittenSubmitResponse>>;

public class SaveWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveWrittenSubmissionCommand, ErrorOr<WrittenSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<WrittenSubmitResponse>> Handle(SaveWrittenSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = _userInfoProvider.AccountId().ThrowIfNull(typeof(UnauthorizedAccessException));

        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
        if (question == null) return Error.NotFound();

        var existingSubmission = await _unitOfWork.WrittenSubmission.GetByAccountIdAsync(
            request.QuestionId, accountId, cancellationToken);

        var submission = new WrittenSubmission
        {
            Answer = request.Answer,
            QuestionId = request.QuestionId,
            AccountId = accountId
        };

        if (existingSubmission is null)
        {
            _unitOfWork.WrittenSubmission.Add(submission);
        }
        else
        {
            existingSubmission.Answer = request.Answer;
            submission = existingSubmission;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return submission.ToDto();
    }
}

public class SaveWrittenSubmissionCommandValidator : AbstractValidator<SaveWrittenSubmissionCommand>
{
    public SaveWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
        
        RuleFor(x => x.Answer).NotEmpty();
    }
}