using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Enum;

public partial class ProgLanguage
{
    public long ProgLanguagesId { get; set; }
    public string Language { get; set; } = null!;

    public ICollection<UserSolution> UserSolutions { get; set; } = [];
}
