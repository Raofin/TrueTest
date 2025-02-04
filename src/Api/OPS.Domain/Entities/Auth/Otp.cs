namespace OPS.Domain.Entities.Auth;

public class Otp
{
    public long OtpId { get; set; }
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public int Attempts { get; set; }
}