using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.User;

public class Otp : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public int Attempts { get; set; }
}