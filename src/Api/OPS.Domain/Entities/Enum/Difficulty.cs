using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public class Difficulty : BaseEntity
{
    public string DifficultyName { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = [];
}