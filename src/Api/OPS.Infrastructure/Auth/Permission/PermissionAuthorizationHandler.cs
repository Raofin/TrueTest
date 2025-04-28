using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces.Auth;

namespace OPS.Infrastructure.Auth.Permission;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var accountId = context.User.Claims.FirstOrDefault(x => x.Type == "AccountId")?.Value;

        if (!Guid.TryParse(accountId, out Guid _))
        {
            return Task.CompletedTask;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var userInfoProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();

        var permissions = userInfoProvider.GetPermissions();

        if (permissions.Contains(requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}