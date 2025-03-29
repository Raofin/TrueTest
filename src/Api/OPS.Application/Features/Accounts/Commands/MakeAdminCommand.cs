using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Commands;

public record MakeAdminCommand(List<Guid> AccountIds) : IRequest<ErrorOr<Success>>;

public class MakeAdminCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<MakeAdminCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(
        MakeAdminCommand request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Account.GetNonAdminAccounts(request.AccountIds, cancellationToken);

        if (accounts.Count == 0)
        {
            return Error.NotFound(description: "No non-admin accounts found.");
        }

        foreach (var account in accounts)
        {
            _unitOfWork.AccountRole.Add(new AccountRole
            {
                AccountId = account.Id,
                RoleId = (int)RoleType.Admin
            });
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class MakeAdminCommandValidator : AbstractValidator<MakeAdminCommand>
{
    public MakeAdminCommandValidator()
    {
        RuleForEach(x => x.AccountIds)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}