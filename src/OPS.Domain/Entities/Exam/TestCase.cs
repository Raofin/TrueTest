namespace OPS.Domain.Entities.Exam;

public partial class TestCase
{
    public long TestCaseId { get; set; }
    public string Input { get; set; } = null!;
    public string Output { get; set; } = null!;

    public long ProblemId { get; set; }
    public Problem Problem { get; set; } = null!;
}
