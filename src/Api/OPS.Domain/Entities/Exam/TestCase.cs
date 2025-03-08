using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class TestCase : BaseEntity
{
    public string Input { get; set; } = null!;
    public string ExpectedOutput { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public ICollection<TestCaseOutput> TestCaseOutputs { get; set; } = [];
}