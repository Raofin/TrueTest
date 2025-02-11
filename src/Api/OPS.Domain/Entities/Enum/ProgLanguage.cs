using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Enum;

public class ProgLanguage : BaseEntity
{
    public string Language { get; set; } = null!;
    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
}