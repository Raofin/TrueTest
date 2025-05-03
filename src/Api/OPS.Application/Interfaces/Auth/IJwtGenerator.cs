using OPS.Domain.Entities.User;

namespace OPS.Application.Interfaces.Auth;

/// <summary>
/// Defines the contract for generating JSON Web Tokens (JWTs).
/// </summary>
public interface IJwtGenerator
{
    /// <summary>
    /// Creates a JWT for the given user account.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity for whom the token is generated.</param>
    /// <returns>
    /// A <see cref="string"/> representing the generated JWT.
    /// </returns>
    string CreateToken(Account account);
}