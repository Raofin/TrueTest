using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Exam;

public class Examination : SoftDeletableEntity
{
    public string Title { get; set; } = null!;
    public string DescriptionMarkdown { get; set; } = null!;
    public decimal TotalPoints { get; set; }
    public decimal ProblemSolvingPoints { get; set; }
    public decimal WrittenPoints { get; set; }
    public decimal McqPoints { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPublished { get; set; }
    public DateTime OpensAt { get; set; }
    public DateTime ClosesAt { get; set; }

    public ICollection<ExamCandidate> ExamCandidates { get; set; } = [];
    public ICollection<Question> Questions { get; set; } = [];
}