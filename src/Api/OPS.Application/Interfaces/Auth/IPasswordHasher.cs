namespace OPS.Application.Interfaces.Auth;

/// <summary>
/// Defines the contract for hashing and verifying passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a raw password, generating a secure hash and a salt.
    /// </summary>
    /// <param name="rawPassword">The plain-text password to hash.</param>
    /// <returns>
    /// A tuple containing the <c>hashedPassword</c> (the secure hash) and the <c>salt</c> used.
    /// </returns>
    (string hashedPassword, string salt) HashPassword(string rawPassword);

    /// <summary>
    /// Verifies if a raw password matches a previously hashed password and salt.
    /// </summary>
    /// <param name="hashedPassword">The previously generated hash of the password.</param>
    /// <param name="salt">The salt used when the password was originally hashed.</param>
    /// <param name="rawPassword">The plain-text password to verify.</param>
    /// <returns>
    /// <c>true</c> if the raw password matches the hashed password and salt; otherwise, <c>false</c>.
    /// </returns>
    bool VerifyPassword(string hashedPassword, string salt, string rawPassword);
}