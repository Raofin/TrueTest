using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Interfaces;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.Authentication.Queries;

public record LoginQuery(string UsernameOrEmail, string Password)
    : IRequest<ErrorOr<AuthenticationResponse>>;

public class LoginQueryHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IAuthService authService) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAuthService _authService = authService;

    public async Task<ErrorOr<AuthenticationResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetWithDetails(request.UsernameOrEmail, cancellationToken);
        if (account == null) return Error.NotFound();

        var isVerified = _passwordHasher.VerifyPassword(account.PasswordHash, account.Salt, request.Password);

        return isVerified
            ? _authService.AuthenticateUser(account)
            : Error.Unauthorized();
    }
}

public class LoginCommandValidator : AbstractValidator<LoginQuery>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty()
            .MinimumLength(4);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(4);
    }
}