namespace OPS.Domain.Common;

public interface IPasswordHasher
{
    (string hashedPassword, string salt) HashPassword(string rawPassword);
    bool VerifyPassword(string hashedPassword, string salt, string rawPassword);
}