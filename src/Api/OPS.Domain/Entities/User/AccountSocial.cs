using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.User;

public class AccountSocial
{
    public long AccountId { get; set; }
    public long SocialLinkId { get; set; }
    public Account Account { get; set; } = null!;
    public SocialLink SocialLink { get; set; } = null!;
}