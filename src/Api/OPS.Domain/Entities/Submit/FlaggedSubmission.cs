using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Submit;

public class FlaggedSubmission : SoftDeletableEntity
{
    public string ReasonMarkdown { get; set; } = null!;

    public Guid ProblemSubmissionId { get; set; }
    public ProblemSubmission ProblemSubmission { get; set; } = null!;
}