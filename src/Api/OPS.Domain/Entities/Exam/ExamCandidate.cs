namespace OPS.Domain.Entities.Exam;

public partial class ExamCandidate
{
    public long ExamCandidateId { get; set; }
    public string UserEmail { get; set; } = null!;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public long ExamId { get; set; }
    public Examination Exam { get; set; } = null!;
}
