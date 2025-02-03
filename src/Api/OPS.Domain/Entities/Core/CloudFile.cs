using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.Core;

public class CloudFile
{
    public long CloudFileId { get; set; }
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Link { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long? AccountId { get; set; }
    public Account? Account { get; set; }
    public ICollection<Account> Accounts { get; set; } = [];
}