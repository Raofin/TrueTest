using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;

namespace OPS.Application.Features.Candidates.Commands;

public record SubmitExamCommand(Guid ExamId) : IRequest<ErrorOr<Success>>;

public class SubmitExamCommandHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
    : IRequestHandler<SubmitExamCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;

    public async Task<ErrorOr<Success>> Handle(SubmitExamCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userProvider.AccountId();
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