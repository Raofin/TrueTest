using System.Security.Cryptography;
using OPS.Application.Interfaces.Auth;

namespace OPS.Infrastructure.Auth;

/// <summary>
/// Implementation of the <see cref="IPasswordHasher"/> interface using BCrypt for password hashing.
/// </summary>
internal class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc />
    public (string hashedPassword, string salt) HashPassword(string rawPassword)
    {
        var salt = GenerateSalt();
        var passwordWithSalt = rawPassword + salt;
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(passwordWithSalt);

        return (hashedPassword, salt);
    }

    /// <inheritdoc />
    public bool VerifyPassword(string hashedPassword, string salt, string rawPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(rawPassword + salt, hashedPassword);
    }

    /// <summary>
    /// Generates a cryptographically secure salt.
    /// </summary>
    /// <returns>A base64 encoded string representing the salt.</returns>
    private static string GenerateSalt()
    {
        var saltBytes = new byte[32];

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }
}