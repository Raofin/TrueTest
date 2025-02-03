namespace OPS.Domain.Entities.Exam;

public class McqAnswer
{
    public long McqAnswerId { get; set; }
    public long QuestionId { get; set; }
    public long McqOptionId { get; set; }
    public McqOption McqOption { get; set; } = null!;
    public Question Question { get; set; } = null!;
}