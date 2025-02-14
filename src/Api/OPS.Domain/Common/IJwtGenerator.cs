using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Common;

public interface IJwtGenerator
{
    string CreateToken(Account account);
}