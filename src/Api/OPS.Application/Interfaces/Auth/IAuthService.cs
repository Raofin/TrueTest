using OPS.Application.Dtos;
using OPS.Domain.Entities.User;

namespace OPS.Application.Interfaces.Auth;

/// <summary>
/// Defines the contract for authentication-related operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user based on their account information.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity containing user credentials.</param>
    /// <returns>
    /// An <see cref="AuthenticationResponse"/> DTO containing the authentication result,
    /// including a token if authentication is successful.
    /// </returns>
    AuthenticationResponse AuthenticateUser(Account account);
}