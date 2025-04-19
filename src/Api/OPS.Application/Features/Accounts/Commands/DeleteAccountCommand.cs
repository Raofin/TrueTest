using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Accounts.Commands;

public record DeleteAccountCommand(Guid AccountId) : IRequest<ErrorOr<Success>>;

public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAccountCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(
        DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (account is null) return Error.NotFound();

        if (account.Username is "rawfin" or "akhi" or "admin")
        {
            return Error.Conflict("This account cannot be deleted.");
        }

        _unitOfWork.Account.Remove(account);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.AccountId).IsValidGuid();
    }
}