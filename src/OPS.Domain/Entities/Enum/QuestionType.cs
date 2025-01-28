using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Enum;

public partial class QuestionType
{
    public long QuestionTypeId { get; set; }
    public string Type { get; set; } = null!;

    public ICollection<Question> Questions { get; set; } = [];
}
