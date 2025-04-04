using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Exam;

public class ExamCandidate : SoftDeletableEntity
{
    public string CandidateEmail { get; set; } = null!;
    public decimal ProblemSolvingScore { get; set; }
    public decimal WrittenScore { get; set; }
    public decimal McqScore { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool HasCheated { get; set; }
    public bool IsReviewed { get; set; }

    public Guid? AccountId { get; set; }
    public Guid ExaminationId { get; set; }
    public Account? Account { get; set; }
    public Examination Examination { get; set; } = null!;
}