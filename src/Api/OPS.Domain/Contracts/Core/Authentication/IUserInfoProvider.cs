namespace OPS.Domain.Contracts.Core.Authentication;

public interface IUserInfoProvider
{
    string? AccountId();
    string? Username();
    string? Email();
    List<string> Roles();
}