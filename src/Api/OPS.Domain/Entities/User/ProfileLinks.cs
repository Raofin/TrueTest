using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.User;

public class ProfileLinks : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}