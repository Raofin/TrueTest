using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces.Auth;

namespace OPS.Infrastructure.Auth.Permission;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    /// <summary>
    /// Handles the authorization requirement asynchronously.
    /// </summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The permission requirement to evaluate.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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