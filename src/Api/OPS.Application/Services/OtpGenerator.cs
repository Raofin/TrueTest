using System.Security.Cryptography;
using OPS.Application.Interfaces.Auth;

namespace OPS.Application.Services;

/// <summary>
/// Implementation of the <see cref="IOtpGenerator"/> interface for generating One-Time Passwords (OTPs).
/// </summary>
public class OtpGenerator : IOtpGenerator
{
    /// <inheritdoc />
    public string Generate(int length = 4)
    {
        if (length is < 4 or > 10)
            throw new ArgumentOutOfRangeException(nameof(length), "OTP length must be between 4 and 10.");

        using var rng = RandomNumberGenerator.Create();

        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);

        var otp = BitConverter.ToInt32(randomBytes, 0) & 0x7FFFFFFF;
        otp %= (int)Math.Pow(10, length);

        return otp.ToString($"D{length}");
    }
}