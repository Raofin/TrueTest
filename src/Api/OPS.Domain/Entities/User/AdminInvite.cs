using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.User;

public class AdminInvite : BaseEntity
{
    public string Email { get; set; } = null!;
}