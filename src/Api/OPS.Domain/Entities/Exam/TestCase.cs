using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Exam;

public class TestCase : SoftDeletableEntity
{
    public string Input { get; set; } = null!;
    public string Output { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}