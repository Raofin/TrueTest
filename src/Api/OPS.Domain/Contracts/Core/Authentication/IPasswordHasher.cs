namespace OPS.Domain.Contracts.Core.Authentication;

public interface IPasswordHasher
{
    (string hashedPassword, string salt) HashPassword(string rawPassword);
    bool VerifyPassword(string hashedPassword, string salt, string rawPassword);
}