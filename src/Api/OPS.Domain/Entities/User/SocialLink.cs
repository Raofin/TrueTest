using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.User;

public class SocialLink
{
    public long SocialLinkId { get; set; }
    public string Link { get; set; } = null!;

    public long SocialTypeId { get; set; }
    public virtual SocialType SocialType { get; set; } = null!;
    public virtual ICollection<AccountSocial> AccountSocials { get; set; } = [];
}