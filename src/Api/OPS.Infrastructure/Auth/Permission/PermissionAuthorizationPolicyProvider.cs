using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace OPS.Infrastructure.Auth.Permission;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(
        string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        return policy ?? new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}