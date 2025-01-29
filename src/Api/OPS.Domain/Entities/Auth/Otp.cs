namespace OPS.Domain.Entities.Auth;

public partial class Otp
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public int Attempts { get; set; }
}
