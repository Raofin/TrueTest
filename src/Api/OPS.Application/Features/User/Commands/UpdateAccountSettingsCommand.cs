using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Constents;
using Throw;

namespace OPS.Application.Features.User.Commands;

public record UpdateAccountSettingsCommand(
    string? Username,
    string? NewPassword,
    string? CurrentPassword) : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class UpdateAccountSettingsCommandHandler(
    IUserProvider userProvider,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateAccountSettingsCommand, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUserProvider _userProvider = userProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(UpdateAccountSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();

        var account = await _unitOfWork.Account.GetWithDetails(userAccountId, cancellationToken);
        account.ThrowIfNull();

        if (!string.IsNullOrWhiteSpace(request.Username) &&
            !request.Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase))
        {
            var isUnique = await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(
                request.Username, null, cancellationToken);

            if (!isUnique) return Error.Conflict(description: "Username is already taken");

            account.Username = request.Username;
        }

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var isVerified = _passwordHasher.VerifyPassword(
                account.PasswordHash,
                account.Salt,
                request.CurrentPassword!
            );

            if (!isVerified) return Error.Forbidden(description: "Invalid current password");

            var (hashedPassword, salt) = _passwordHasher.HashPassword(request.NewPassword);
            account.PasswordHash = hashedPassword;
            account.Salt = salt;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return account.MapToDtoWithDetails();
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