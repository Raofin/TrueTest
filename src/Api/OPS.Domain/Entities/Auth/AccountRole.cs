using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.Auth;

public class AccountRole
{
    public Guid AccountId { get; set; }
    public int RoleTypeId { get; set; }
    public Account Account { get; set; } = null!;
    public RoleType RoleType { get; set; } = null!;
}