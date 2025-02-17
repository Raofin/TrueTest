using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Core.Authentication;

public interface IJwtGenerator
{
    string CreateToken(Account account);
}