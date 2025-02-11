using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.User;

public class SocialLink : BaseEntity
{
    public string Link { get; set; } = null!;

    public SocialType SocialType { get; set; } = null!;
    public ICollection<ProfileSocial> ProfileSocials { get; set; } = [];
}