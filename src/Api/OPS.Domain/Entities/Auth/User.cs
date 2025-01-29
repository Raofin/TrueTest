using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Auth;

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
    public CloudFile? CloudFile { get; set; }
    public Otp? Otp { get; set; }
    public UserDetail? UserDetail { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<Social> Socials { get; set; } = [];
    public ICollection<UserSolution> UserSolutions { get; set; } = [];
    public ICollection<UserWrittenAnswer> UserWrittenAnswers { get; set; } = [];
    public ICollection<UserMcqAnswer> UserMcqAnswers { get; set; } = [];
    public ICollection<CloudFile> CloudFiles { get; set; } = [];
}
