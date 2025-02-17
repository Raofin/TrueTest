using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class McqOption : BaseEntity
{
    public string Option1 { get; set; } = null!;
    public string Option2 { get; set; } = null!;
    public string? Option3 { get; set; }
    public string? Option4 { get; set; }
    public bool IsMultiSelect { get; set; }
    public string AnswerOptions { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
}