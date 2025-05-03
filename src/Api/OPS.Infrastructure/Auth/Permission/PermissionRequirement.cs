using Microsoft.AspNetCore.Authorization;

namespace OPS.Infrastructure.Auth.Permission;

/// <summary>
/// Represents a requirement for a specific permission to be authorized.
/// </summary>
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    /// <summary>
    /// The required permission.
    /// </summary>
    public string Permission { get; } = permission;
}