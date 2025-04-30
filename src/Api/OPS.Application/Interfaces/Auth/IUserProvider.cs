using OPS.Application.Dtos;

namespace OPS.Application.Interfaces.Auth;

/// <summary>
/// Provides access to information about the current user and their authentication status.
/// </summary>
public interface IUserProvider
{
    /// <summary>
    /// Checks if the current user is authenticated.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the user is authenticated; otherwise, <c>false</c>.
    /// </returns>
    bool IsAuthenticated();

    /// <summary>
    /// Retrieves information about the currently logged-in user.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="CurrentUser"/> DTO containing user details.
    /// Returns <c>null</c> if the user is not authenticated.
    /// </returns>
    CurrentUser GetCurrentUser();

    /// <summary>
    /// Retrieves a list of permissions granted to the current user.
    /// </summary>
    /// <returns>
    /// A <see>
    ///     <cref>List{string}</cref>
    /// </see>
    /// containing the names of the user's permissions.
    /// Returns an empty list if the user has no specific permissions or is not authenticated.
    /// </returns>
    List<string> GetPermissions();

    /// <summary>
    /// Retrieves the unique identifier of the current user's account.
    /// </summary>
    /// <returns>
    /// A <see cref="Guid"/> representing the account ID.
    /// </returns>
    Guid AccountId();

    /// <summary>
    /// Attempts to retrieve the unique identifier of the current user's account.
    /// </summary>
    /// <returns>
    /// A <see cref="Guid"/> representing the account ID if available; otherwise, <c>null</c>.
    /// </returns>
    Guid? TryGetAccountId();

    /// <summary>
    /// Decodes the authentication token associated with the current user.
    /// </summary>
    /// <returns>
    /// A dynamic object representing the decoded token claims.
    /// The structure of this object depends on the specific token format used.
    /// Returns <c>null</c> if no token is available or if decoding fails.
    /// </returns>
    dynamic DecodeToken();
}