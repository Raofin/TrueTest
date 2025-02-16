namespace OPS.Application.Contracts.Dtos;

public record AuthenticationResult(
    AccountResponse Account,
    string Token
);

public record AccountResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive
);