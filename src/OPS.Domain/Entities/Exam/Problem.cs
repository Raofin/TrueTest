using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Exam;

public partial class Problem
{
    public long ProblemId { get; set; }
    public string DifficultyLevel { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long? QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public ICollection<TestCase> TestCases { get; set; } = [];
    public ICollection<UserSolution> UserSolutions { get; set; } = [];
}
