using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Exam;

public partial class McqOption
{
    public long McqOptionId { get; set; }
    public string Optionn { get; set; } = null!;

    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<UserMcqAnswer> UserMcqAnswers { get; set; } = [];
}
