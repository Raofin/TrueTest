namespace OPS.Domain.Entities.Usr;

public partial class UserSocial
{
    public long UserId { get; set; }

    public long SocialId { get; set; }
    public Social Social { get; set; } = null!;
}
