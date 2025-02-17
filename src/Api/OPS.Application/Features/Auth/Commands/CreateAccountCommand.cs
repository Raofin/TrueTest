using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Auth;
using OPS.Application.Extensions;
using OPS.Domain;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Features.Auth.Commands;

public record CreateAccountCommand(
    string Username,
    string Email,
    bool IsVerified,
    string PasswordHash,
    string Salt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    bool IsDeleted
) : IRequest<ErrorOr<AccountResponse>>;

public class CreateAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAccountCommand, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountResponse>> Handle(CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var Account = new Account
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            Salt = request.Salt,
            IsVerified = request.IsVerified,
            CreatedAt = DateTime.UtcNow,
            IsActive = request.IsActive,
            UpdatedAt = DateTime.UtcNow
        };

        _unitOfWork.Account.Add(Account);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Account.ToDto()
            : Error.Failure("The Account could not be saved.");
    }
}

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Length(10, 500).WithMessage("Email must be between 10 and 500 characters.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .Length(10, 500).WithMessage("Password must be between 6 and 20 characters.");
    }
}