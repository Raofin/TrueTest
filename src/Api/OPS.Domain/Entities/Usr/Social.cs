using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.Usr;

public partial class Social
{
    public long SocialId { get; set; }
    public long SocialPlatformId { get; set; }
    public string Link { get; set; } = null!;

    public SocialPlatform SocialPlatform { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}