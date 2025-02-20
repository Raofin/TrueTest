using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public class Difficulty
{
    public int Id { get; set; }
    public string DifficultyName { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = [];
}