using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using Throw;

namespace OPS.Application.Features.User.Commands;

public record UpdateAccountSettingsCommand(
    string? Username,
    string? NewPassword,
    string? CurrentPassword) : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class UpdateAccountSettingsCommandHandler(
    IUserInfoProvider userInfoProvider,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateAccountSettingsCommand, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(UpdateAccountSettingsCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var account = await _unitOfWork.Account.GetAsync(userAccountId, cancellationToken);
        account.ThrowIfNull();

        if (request.Username is not null)
        {
            var isUnique = await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(request.Username, null, cancellationToken);

            if (!isUnique) return Error.Conflict(description: "Username is already taken");

            account.Username = request.Username;
        }

        if (request.NewPassword is not null)
        {
            var isVerified = _passwordHasher.VerifyPassword(account.PasswordHash, account.Salt, request.CurrentPassword!);
            if (!isVerified) return Error.Unauthorized(description: "Invalid current password");

            var (hashedPassword, salt) = _passwordHasher.HashPassword(request.NewPassword);
            account.PasswordHash = hashedPassword;
            account.Salt = salt;
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.ToDtoWithDetails()
            : Error.Failure();
    }
}

public class UpdateAccountSettingsCommandValidator : AbstractValidator<UpdateAccountSettingsCommand>
{
    public UpdateAccountSettingsCommandValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(4)
            .When(x => !string.IsNullOrEmpty(x.Username));

        RuleFor(x => x.NewPassword)
            .Matches(ValidationConstants.PasswordRegex)
            .When(x => !string.IsNullOrEmpty(x.NewPassword));

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.NewPassword));
    }
}