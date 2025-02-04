using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Submit;

public class WrittenSubmission
{
    public long WrittenSubmissionId { get; set; }
    public string Answer { get; set; } = null!;
    public decimal Score { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public long QuestionId { get; set; }
    public long AccountId { get; set; }
    public Question Question { get; set; } = null!;
    public Account Account { get; set; } = null!;
}