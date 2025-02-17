using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Exam;

public class ExamCandidate : SoftDeletableEntity
{
    public string CandidateEmail { get; set; } = null!;
    public decimal Score { get; set; }
    public DateTime? StartedAt { get; set; } = null;
    public DateTime? SubmittedAt { get; set; } = null;
    public bool HasCheated { get; set; } = false;

    public Guid? AccountId { get; set; }
    public Guid ExaminationId { get; set; }
    public Account? Account { get; set; }
    public Examination Examination { get; set; } = null!;
}