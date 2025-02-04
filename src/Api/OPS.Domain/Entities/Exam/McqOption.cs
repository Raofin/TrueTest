using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class McqOption
{
    public long McqOptionId { get; set; }
    public long QuestionId { get; set; }
    public string OptionMarkdown { get; set; }
    public Question Question { get; set; }
    public ICollection<McqAnswer> McqAnswers { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
}