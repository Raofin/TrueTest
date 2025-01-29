using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.Enum;

public partial class Role
{
    public long RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public ICollection<User> Users { get; set; } = [];
}
