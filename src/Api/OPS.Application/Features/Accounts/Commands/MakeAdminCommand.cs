using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Commands;

public record MakeAdminCommand(Guid AccountId) : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class MakeAdminCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<MakeAdminCommand, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(
        MakeAdminCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithDetails(request.AccountId, cancellationToken);
        if (account is null) return Error.NotFound();

        if (account.AccountRoles.Any(q => q.RoleId == (int)RoleType.Admin))
        {
            return account.ToDtoWithDetails();
        }

        var accountRole = new AccountRole
        {
            AccountId = request.AccountId,
            RoleId = (int)RoleType.Admin
        };

        account.AccountRoles.Add(accountRole);

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.ToDtoWithDetails()
            : Error.Failure();
    }
}

public class MakeAdminCommandValidator : AbstractValidator<MakeAdminCommand>
{
    public MakeAdminCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
    }
}