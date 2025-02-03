using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Submit;

public class McqSubmission
{
    public long McqSubmissionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public long AccountId { get; set; }
    public long QuestionId { get; set; }
    public long McqOptionId { get; set; }
    public virtual Account Account { get; set; } = null!;
    public virtual McqOption McqOption { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
}