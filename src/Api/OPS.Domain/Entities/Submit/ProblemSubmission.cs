using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Submit;

public class ProblemSubmission : BaseEntity
{
    public string Code { get; set; } = null!;
    public int Attempts { get; set; }
    public decimal Score { get; set; }
    public bool IsFlagged { get; set; }
    public string? FlagReason { get; set; }

    public int ProgLanguageId { get; set; }
    public Guid AccountId { get; set; }
    public Guid QuestionId { get; set; }
    public Account Account { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public ProgLanguage ProgLanguages { get; set; } = null!;
}