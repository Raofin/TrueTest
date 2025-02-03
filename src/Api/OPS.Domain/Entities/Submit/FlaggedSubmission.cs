namespace OPS.Domain.Entities.Submit;

public class FlaggedSubmission
{
    public long FlaggedSolutionId { get; set; }
    public string ReasonMarkdown { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long ProblemSubmissionId { get; set; }
    public ProblemSubmission ProblemSubmission { get; set; } = null!;
}