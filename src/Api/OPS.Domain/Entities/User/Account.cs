using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.User;

public class Account : SoftDeletableEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public Profile? Profile { get; set; }
    public ICollection<AccountRole> AccountRoles { get; set; } = [];
    public ICollection<ExamCandidate> ExamCandidates { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
    public ICollection<WrittenSubmission> WrittenSubmissions { get; set; } = [];
    public ICollection<CloudFile> CloudFiles { get; set; } = [];
}