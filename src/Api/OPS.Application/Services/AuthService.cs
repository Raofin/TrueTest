using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain.Entities.User;

namespace OPS.Application.Services;

/// <summary>
/// Implementation of the <see cref="IAuthService"/> interface for handling user authentication.
/// </summary>
internal class AuthService(IJwtGenerator jwtGenerator) : IAuthService
{
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    /// <inheritdoc />
    public AuthenticationResponse AuthenticateUser(Account account)
    {
        return new AuthenticationResponse(_jwtGenerator.CreateToken(account), account.MapToDtoWithDetails());
    }
}