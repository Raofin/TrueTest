﻿using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Constants;

namespace OPS.Application.Features.Accounts.Commands;

public record UpdateAccountCommand(
    Guid AccountId,
    string? Username,
    string? Email) : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class UpdateAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(
        UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithDetails(command.AccountId, cancellationToken);
        if (account is null) return Error.NotFound();

        var isUnique =
            await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(
                command.Username,
                command.Email,
                cancellationToken
            );

        if (!isUnique) return Error.Conflict();

        account.Username = command.Username ?? account.Username;
        account.Email = command.Email ?? account.Email;

        await _unitOfWork.CommitAsync(cancellationToken);

        return account.MapToDtoWithDetails();
    }
}

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .IsValidGuid();

        RuleFor(x => x.Username)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Username))
            .MinimumLength(4)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .Matches(ValidationConstants.EmailRegex);
    }
}