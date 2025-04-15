using Microsoft.AspNetCore.Authorization;

namespace OPS.Infrastructure.Authentication.Permission;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}