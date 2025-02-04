using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Submit;

public class ProblemSubmission
{
    public long ProblemSubmissionId { get; set; }

    public string Code { get; set; } = null!;
    public int Attempts { get; set; }
    public decimal Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long AccountId { get; set; }
    public long QuestionId { get; set; }
    public long ProgLanguagesId { get; set; }
    public Account Account { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public ProgLanguage ProgLanguages { get; set; } = null!;
    public ICollection<FlaggedSubmission> FlaggedSubmissions { get; set; } = [];
}