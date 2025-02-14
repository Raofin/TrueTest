using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class McqOption : BaseEntity
{
    public string OptionMarkdown { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public ICollection<McqAnswer> McqAnswers { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
}