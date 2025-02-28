using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Authentication.Commands;

public record ResetPasswordCommand(
    string Email,
    string Password,
    string Otp) : IRequest<ErrorOr<AuthenticationResult>>;

public class ResetPasswordCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtGenerator jwtGenerator) : IRequestHandler<ResetPasswordCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    public async Task<ErrorOr<AuthenticationResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithDetails(request.Email, cancellationToken);

        if (account == null) return Error.NotFound();

        var isValidOtp = await _unitOfWork.Otp.IsValidOtpAsync(request.Email, request.Otp, cancellationToken);

        if (!isValidOtp)
            return Error.Unauthorized(description: "Invalid OTP.");

        var (hashedPassword, salt) = _passwordHasher.HashPassword(request.Password);


        account.PasswordHash = hashedPassword;
        account.Salt = salt;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? new AuthenticationResult(_jwtGenerator.CreateToken(account), account.ToDto())
            : Error.Failure();
    }
}

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(ValidationConstants.EmailRegex);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(ValidationConstants.PasswordRegex)
            .WithMessage("Password must be at least 8 chars long, contain at least 1x (lowercase, uppercase, digit, special char).");

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(4);
    }
}