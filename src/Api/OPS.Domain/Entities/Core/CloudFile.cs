using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Core;

public class CloudFile : SoftDeletableEntity
{
    public string FileId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }

    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }
    public Profile? Profile { get; set; }
}