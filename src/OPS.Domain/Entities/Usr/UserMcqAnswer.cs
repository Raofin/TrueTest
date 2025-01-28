using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Usr;

public partial class UserMcqAnswer
{
    public long UserMcqAnswerId { get; set; }
    public long UserId { get; set; }
    public long QuestionId { get; set; }
    public long McqOptionId { get; set; }

    public User User { get; set; } = null!;
    public McqOption McqOption { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
