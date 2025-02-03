namespace OPS.Domain.Entities.Exam;

public class TestCase
{
    public long TestCaseId { get; set; }
    public string Input { get; set; } = null!;
    public string Output { get; set; } = null!;
    public long QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}