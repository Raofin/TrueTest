using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.Candidates.Commands;

public record SubmitExamCommand(Guid ExamId) : IRequest<ErrorOr<Success>>;

public class SubmitExamCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SubmitExamCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<Success>> Handle(SubmitExamCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userInfoProvider.AccountId();
        var examCandidate = await _unitOfWork.ExamCandidate.GetAsync(accountId, request.ExamId, cancellationToken);
        if (examCandidate is null) return Error.Unexpected();

        examCandidate.SubmittedAt = DateTime.UtcNow;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class SubmitExamCommandValidator : AbstractValidator<SubmitExamCommand>
{
    public SubmitExamCommandValidator()
    {
        RuleFor(x => x.ExamId).IsValidGuid();
    }
}