namespace OPS.Domain.Entities.Exam;

public class McqAnswer
{
    public long QuestionId { get; set; }
    public long McqOptionId { get; set; }
    public Question Question { get; set; } = null!;
    public McqOption McqOption { get; set; } = null!;
}