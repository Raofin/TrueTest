using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Common;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Features.User.Queries;

public record AuthenticationResult(Account Account, string Token);

public record LoginQuery(string UsernameOrEmail, string Password)
    : IRequest<ErrorOr<AuthenticationResult>>;

public class LoginQueryHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtGenerator jwtGenerator) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithDetails(request.UsernameOrEmail, cancellationToken);

        if (account == null) return Error.NotFound();

        var isVerified = _passwordHasher.VerifyPassword(account.PasswordHash, account.Salt, request.Password);

        return isVerified
            ? new AuthenticationResult(account, _jwtGenerator.CreateToken(account))
            : Error.Unauthorized();
    }
}

public class LoginCommandValidator : AbstractValidator<LoginQuery>
{
    public LoginCommandValidator()
    {
        RuleFor(a => a.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or email is required.");

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}