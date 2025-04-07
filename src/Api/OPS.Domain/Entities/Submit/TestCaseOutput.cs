using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Submit;

public class TestCaseOutput : BaseEntity
{
    public string ReceivedOutput { get; set; } = null!;
    public bool IsAccepted { get; set; }
    public Guid TestCaseId { get; set; }
    public Guid ProblemSubmissionId { get; set; }
    public TestCase TestCase { get; set; } = null!;
    public ProblemSubmission ProblemSubmissions { get; set; } = null!;
}