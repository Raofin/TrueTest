using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public class QuestionType : BaseEntity
{
    public string Type { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = [];
}