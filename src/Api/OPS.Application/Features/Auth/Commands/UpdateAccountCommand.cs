using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Auth;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Commands;

public record UpdateProfileCommand(
    long AccountId,
    string? Username,
    string? Email,
    string? PasswordHash,   
    string? Salt,   
    DateTime? UpdatedAt,
    bool? IsVerified,
    bool? IsActive,
    bool? IsDeleted
) : IRequest<ErrorOr<AccountResponse>>;

public class UpdateAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProfileCommand, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountResponse>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetAsync(command.AccountId, cancellationToken);

        if (account is null) return Error.NotFound("Account was not found");

        account.Username = command.Username ?? account.Username;
        account.Email = command.Email ?? account.Email;
        account.PasswordHash = command.PasswordHash ?? account.PasswordHash;
        account.Salt = command.Salt ?? account.Salt;        
        account.UpdatedAt = DateTime.UtcNow;
        account.IsActive = command.IsActive ?? account.IsActive;
        account.IsDeleted = command.IsDeleted ?? account.IsDeleted;
        account.IsVerified = command.IsVerified ?? account.IsVerified;      


        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.ToDto()
            : Error.Failure("The Account could not be saved.");
    }
}

public class UpdateAccountCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .GreaterThan(0).WithMessage("AccountId must be a positive number.");

        RuleFor(x => x.Username)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Username))
            .WithMessage("Username cannot exceed 100 characters.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .Length(10, 500).WithMessage("Password must be between 6 and 20 characters.");

        RuleFor(x => x.UpdatedAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.UpdatedAt.HasValue)
            .WithMessage("UpdateAt must be in the future.");

        RuleFor(x => x.IsActive)
            .NotNull().When(x => x.IsActive.HasValue)
            .WithMessage("IsActive flag is required when provided.");

        RuleFor(x => x.IsDeleted)
            .NotNull().When(x => x.IsDeleted.HasValue)
            .WithMessage("IsDeleted flag is required when provided.");
    }
}