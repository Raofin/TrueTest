using OPS.Domain.Entities.Enum;

namespace OPS.Domain.Entities.User;

public class AccountRole
{
    public Guid AccountId { get; set; }
    public int RoleId { get; set; }
    public Account Account { get; set; } = null!;
    public Role Role { get; set; } = null!;
}