using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Common;

namespace OPS.Domain.Entities.Enum;

public class RoleType : BaseEntity
{
    public string RoleName { get; set; } = null!;
    public ICollection<AccountRole> AccountRoles { get; set; } = [];
}