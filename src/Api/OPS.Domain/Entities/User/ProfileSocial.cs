using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.User;

public class ProfileSocial
{
    public Guid ProfileId { get; set; }
    public Guid SocialLinkId { get; set; }
    public Profile Profile { get; set; } = null!;
    public SocialLink SocialLink { get; set; } = null!;
}