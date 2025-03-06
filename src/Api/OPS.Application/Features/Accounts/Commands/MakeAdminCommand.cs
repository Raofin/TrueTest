using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Commands;

public record MakeAdminCommand(Guid AccountId) : IRequest<ErrorOr<AccountResponse>>;

public class MakeAdminCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<MakeAdminCommand, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountResponse>> Handle(MakeAdminCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithProfile(request.AccountId, cancellationToken);

        if (account is null)
        {
            return Error.NotFound();
        }

        if (account.AccountRoles.Any(q => q.RoleId == (int)RoleType.Admin))
        {
            return account.ToDto();
        }

        var accountRole = new AccountRole
        {
            AccountId = request.AccountId,
            RoleId = (int)RoleType.Admin
        };

        account.AccountRoles.Add(accountRole);

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.ToDto()
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