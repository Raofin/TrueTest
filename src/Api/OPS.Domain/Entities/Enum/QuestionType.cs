using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public class QuestionType
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = [];
}