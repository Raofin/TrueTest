using Microsoft.AspNetCore.Authorization;
using OPS.Domain.Enums;

namespace OPS.Application.CrossCutting.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params RoleType[] roles)
    {
        Roles = string.Join(",", roles);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeCandidateAttribute : AuthorizeAttribute
{
    public AuthorizeCandidateAttribute()
    {
        Roles = nameof(RoleType.Candidate);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeModeratorAttribute : AuthorizeAttribute
{
    public AuthorizeModeratorAttribute()
    {
        Roles = nameof(RoleType.Moderator);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAdminAttribute : AuthorizeAttribute
{
    public AuthorizeAdminAttribute()
    {
        Roles = nameof(RoleType.Admin);
    }
}