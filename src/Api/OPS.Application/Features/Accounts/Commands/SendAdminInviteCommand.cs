using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Commands;

public record SendAdminInviteCommand(List<string> Email) : IRequest<ErrorOr<Success>>;

public class SendAdminInviteCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SendAdminInviteCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(SendAdminInviteCommand request, CancellationToken cancellationToken)
    {
        var emails = await _unitOfWork.AdminInvite.GetUninvitedEmails(request.Email, cancellationToken);
        var existingAccounts = await _unitOfWork.Account.GetByEmailsAsync(emails, cancellationToken);

        var nonAdminEmails = existingAccounts
            .Where(ea => ea.AccountRoles.All(ar => ar.RoleId != (int)RoleType.Admin))
            .ToList();

        var newAdminRoles = nonAdminEmails.Select(account => new AccountRole
            { AccountId = account.Id, RoleId = (int)RoleType.Admin }
        );

        _unitOfWork.AccountRole.AddRange(newAdminRoles);
        var updatedAccountEmails = nonAdminEmails.Select(a => a.Email).ToList();

        emails = emails.Except(existingAccounts.Select(e => e.Email)).ToList();

        var adminInvites = emails.Select(email => new AdminInvite { Email = email }).ToList();
        _unitOfWork.AdminInvite.AddRange(adminInvites);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class SendAdminInviteCommandValidator : AbstractValidator<SendAdminInviteCommand>
{
    public SendAdminInviteCommandValidator()
    {
        RuleForEach(x => x.Email)
            .NotEmpty()
            .Matches(ValidationConstants.EmailRegex);
    }
}