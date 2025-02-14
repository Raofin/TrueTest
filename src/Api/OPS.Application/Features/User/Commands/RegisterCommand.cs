using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Features.User.Commands;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string Otp) : IRequest<ErrorOr<AuthenticationResult>>;

public class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtGenerator jwtGenerator) : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Account.IsUsernameOrEmailTakenAsync(request.Username, request.Email, cancellationToken))
            return Error.Conflict(description: "Username or Email is already taken.");

        var (hashedPassword, salt) = _passwordHasher.HashPassword(request.Password);

        var account = new Account
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword,
            Salt = salt,
            IsVerified = true
        };

        _unitOfWork.Account.Add(account);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        if (result <= 0) return Error.Failure(description: "Account could not be saved.");

        return new AuthenticationResult(account.ToDto(), _jwtGenerator.CreateToken(account));
    }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("Password must be at least 8 characters long, contain at least one letter, one number, and one special character.");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("Otp is required.")
            .MinimumLength(4).WithMessage("Otp must be at least 4 characters.")
            .MaximumLength(4).WithMessage("Otp must not exceed 4 characters.");
    }
}