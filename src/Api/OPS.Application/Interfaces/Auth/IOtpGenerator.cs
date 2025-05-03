namespace OPS.Application.Interfaces.Auth;

/// <summary>
/// Defines the contract for generating One-Time Passwords (OTPs).
/// </summary>
public interface IOtpGenerator
{
    /// <summary>
    /// Generates a random OTP of the specified length.
    /// </summary>
    /// <param name="length">The desired length of the OTP. Defaults to 4.</param>
    /// <returns>
    /// A <see cref="string"/> representing the generated OTP.
    /// </returns>
    string Generate(int length = 4);
}