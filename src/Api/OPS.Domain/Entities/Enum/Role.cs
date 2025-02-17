using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Enum;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    public ICollection<AccountRole> AccountRoles { get; set; } = [];
}