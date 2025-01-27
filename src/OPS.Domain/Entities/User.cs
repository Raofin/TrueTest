namespace OPS.Domain.Entities;

public partial class User
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public long? CloudFileId { get; set; }

    public virtual UserDetail? UserDetail { get; set; }
}
