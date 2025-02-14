using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Contracts.Core.Authentication;

public interface IJwtGenerator
{
    string CreateToken(Account account);
}