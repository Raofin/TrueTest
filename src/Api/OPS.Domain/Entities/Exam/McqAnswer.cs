using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Exam;

public class McqAnswer : BaseEntity
{
    public Guid QuestionId { get; set; }
    public Guid McqOptionId { get; set; }
    public McqOption McqOption { get; set; } = null!;
    public Question Question { get; set; } = null!;
}