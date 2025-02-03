using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Enum;

public class SocialType
{
    public long SocialTypeId { get; set; }
    public string PlatformName { get; set; } = null!;

    public ICollection<SocialLink> SocialLinks { get; set; } = [];
}