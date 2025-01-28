using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Usr;

public partial class UserSolution
{
    public long UserSolutionId { get; set; }
    public string Code { get; set; } = null!;
    public string Language { get; set; } = null!;
    public int Attempts { get; set; }
    public decimal Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long UserId { get; set; }
    public long ProblemId { get; set; }
    public long ProgLanguagesId { get; set; }
    public Problem Problem { get; set; } = null!;
    public ProgLanguage ProgLanguages { get; set; } = null!;
    public User User { get; set; } = null!;

    public ICollection<FlaggedSolution> FlaggedSolutions { get; set; } = [];
}
