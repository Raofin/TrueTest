using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Usr;

public partial class UserWrittenAnswer
{
    public long UserWrittenAnswerId { get; set; }
    public string Answer { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long UserId { get; set; }
    public long QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
