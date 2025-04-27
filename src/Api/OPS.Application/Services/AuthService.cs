using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain.Entities.User;

namespace OPS.Application.Services;

internal class AuthService(IJwtGenerator jwtGenerator) : IAuthService
{
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    public AuthenticationResponse AuthenticateUser(Account account)
    {
        return new AuthenticationResponse(_jwtGenerator.CreateToken(account), account.MapToDtoWithDetails());
    }
}