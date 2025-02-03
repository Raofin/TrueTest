namespace OPS.Domain.Entities.Exam;

public class Examination
{
    public long ExaminationId { get; set; }
    public string Title { get; set; } = null!;
    public string DescriptionMarkdown { get; set; } = null!;
    public DateTime OpensAt { get; set; }
    public DateTime ClosesAt { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<ExamCandidate> ExamCandidates { get; set; } = [];
    public virtual ICollection<Question> Questions { get; set; } = [];
}