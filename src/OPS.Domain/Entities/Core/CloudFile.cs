using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.Core;

public partial class CloudFile
{
    public long CloudFileId { get; set; }
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public string Link { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public long? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<User> Users { get; set; } = [];
}
