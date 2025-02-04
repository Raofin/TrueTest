using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Auth;

public class Account
{
    public long AccountId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsVerified { get; set; }
    public long? CloudFileId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public Profile Profile { get; set; }
    public CloudFile CloudFile { get; set; }
    public ICollection<Otp> Otps { get; set; } = [];
    public ICollection<AccountRole> AccountRoles { get; set; } = [];
    public ICollection<AccountSocial> AccountSocials { get; set; } = [];
    public ICollection<CloudFile> CloudFiles { get; set; } = [];
    public ICollection<ExamCandidate> ExamCandidates { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
    public ICollection<WrittenSubmission> WrittenSubmissions { get; set; } = [];
}