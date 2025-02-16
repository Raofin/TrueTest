using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Features.Auth.Commands;

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
        var isUserUnique =
            await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(
                request.Username,
                request.Email,
                cancellationToken
            );

        if (!isUserUnique)
            return Error.Conflict();

        var isValidOtp = await _unitOfWork.Otp.IsValidOtpAsync(request.Email, request.Otp, cancellationToken);

        if (!isValidOtp)
            return Error.Unauthorized();

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

        return result > 0
            ? new AuthenticationResult(account.ToDto(), _jwtGenerator.CreateToken(account))
            : Error.Failure();
    }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("Password must be at least 8 characters long, contain at least one letter, one number, and one special character.");

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(4);
    }
}