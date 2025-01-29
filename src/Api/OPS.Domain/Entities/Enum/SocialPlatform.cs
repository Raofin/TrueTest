using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Enum;

public partial class SocialPlatform
{
    public long SocialPlatformId { get; set; }
    public string PlatformName { get; set; } = null!;

    public ICollection<Social> Socials { get; set; } = [];
}
