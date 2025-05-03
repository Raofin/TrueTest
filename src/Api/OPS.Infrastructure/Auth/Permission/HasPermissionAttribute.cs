using Microsoft.AspNetCore.Authorization;

namespace OPS.Infrastructure.Auth.Permission;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission);