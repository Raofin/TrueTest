using Microsoft.Identity.Client;

namespace OPS.Domain.Common;

public interface IAuthentication
{
    Task SignInAsync(AuthenticationResult authentication);
    Task SignOutAsync();
}