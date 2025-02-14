using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Core;

public class CloudFile : SoftDeletableEntity
{
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Link { get; set; } = null!;

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Profile? Profile { get; set; }
}