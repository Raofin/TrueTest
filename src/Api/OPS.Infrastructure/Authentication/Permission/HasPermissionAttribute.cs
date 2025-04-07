using Microsoft.AspNetCore.Authorization;

namespace OPS.Infrastructure.Authentication.Permission;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission);