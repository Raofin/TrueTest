using System.Security.Cryptography;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Infrastructure.Authentication;

internal class PasswordHasher : IPasswordHasher
{
    public (string hashedPassword, string salt) HashPassword(string rawPassword)
    {
        var salt = GenerateSalt();
        var passwordWithSalt = rawPassword + salt;
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(passwordWithSalt);

        return (hashedPassword, salt);
    }

    public bool VerifyPassword(string hashedPassword, string salt, string rawPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(rawPassword + salt, hashedPassword);
    }

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