using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Commands;

public record SendAdminInviteCommand(string Email) : IRequest<ErrorOr<Success>>;

public class SendAdminInviteCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SendAdminInviteCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(SendAdminInviteCommand request, CancellationToken cancellationToken)
    {
        var isExists = await _unitOfWork.AdminInvite.IsExistsAsync(request.Email, cancellationToken);
        if (isExists) return Result.Success;

        var account = await _unitOfWork.Account.GetByEmailAsync(request.Email, cancellationToken);

        if (account is null)
        {
            var adminInvite = new AdminInvite { Email = request.Email };
            _unitOfWork.AdminInvite.Add(adminInvite);
        }
        else if (account.AccountRoles.All(role => role.RoleId != (int)RoleType.Admin))
        {
            var accountRole = new AccountRole
            {
                AccountId = account.Id,
                RoleId = (int)RoleType.Admin
            };

            account.AccountRoles.Add(accountRole);
        }
        else
        {
            return Result.Success;
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class SendAdminInviteCommandValidator : AbstractValidator<SendAdminInviteCommand>
{
    public SendAdminInviteCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(ValidationConstants.EmailRegex);
    }
}