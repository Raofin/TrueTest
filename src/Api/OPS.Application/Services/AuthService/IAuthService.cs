using OPS.Application.Dtos;
using OPS.Domain.Entities.User;

namespace OPS.Application.Services.AuthService;

public interface IAuthService
{
    AuthenticationResponse AuthenticateUser(Account account);
}