using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Enum;

public class SocialType : BaseEntity
{
    public string PlatformName { get; set; } = null!;
    public ICollection<SocialLink> SocialLinks { get; set; } = [];
}