using OPS.Application.Dtos;
using OPS.Domain.Entities.User;

namespace OPS.Application.Interfaces.Auth;

public interface IAuthService
{
    AuthenticationResponse AuthenticateUser(Account account);
}