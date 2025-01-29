using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.Auth;

public partial class UserRole
{
    public long UserId { get; set; }

    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
