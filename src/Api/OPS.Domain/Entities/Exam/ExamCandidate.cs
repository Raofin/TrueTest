using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.Exam;

public class ExamCandidate
{
    public long ExamCandidateId { get; set; }
    public string CandidateEmail { get; set; } = null!;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public long? AccountId { get; set; }
    public long ExaminationId { get; set; }
    public Account? Account { get; set; }
    public Examination Examination { get; set; } = null!;
}