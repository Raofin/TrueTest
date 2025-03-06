using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.User;

public class AdminInvites : BaseEntity
{
    public string Email { get; set; } = null!;
}