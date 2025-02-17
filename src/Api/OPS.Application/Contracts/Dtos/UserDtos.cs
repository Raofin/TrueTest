namespace OPS.Application.Contracts.Dtos;

public record AuthenticationResult(
    string Token,
    AccountResponse Account
);

public record AccountResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);