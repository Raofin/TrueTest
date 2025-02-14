using OPS.Domain.Entities.Auth;

namespace OPS.Application.Contracts.Dtos;

public record AuthenticationResult(
    AccountResponse Account,
    string Token
);

public record AccountResponse(
    Guid Id,
    string Username,
    string Email,
    bool IsVerified,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive
);