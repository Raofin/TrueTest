using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.Enum;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    public ICollection<AccountRole> AccountRoles { get; set; } = [];
}