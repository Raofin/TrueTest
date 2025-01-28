using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Exam;

public partial class FlaggedSolution
{
    public long FlaggedSolutionId { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long UserSolutionId { get; set; }
    public UserSolution UserSolution { get; set; } = null!;
}
