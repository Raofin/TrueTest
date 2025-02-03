using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Enum;

public class ProgLanguage
{
    public long ProgLanguageId { get; set; }
    public string Language { get; set; } = null!;

    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
}