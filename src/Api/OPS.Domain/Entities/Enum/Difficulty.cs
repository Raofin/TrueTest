using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public class Difficulty
{
    public long DifficultyId { get; set; }
    public string DifficultyName { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = [];
}