using OPS.Domain.Entities.User;

namespace OPS.Application.Interfaces.Auth;

public interface IJwtGenerator
{
    string CreateToken(Account account);
}