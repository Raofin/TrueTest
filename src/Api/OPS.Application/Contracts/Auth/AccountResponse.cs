namespace OPS.Application.Contracts.Auth;

public record AccountResponse(
    Guid Id,
    string Username,
    string Email,
    string PasswordHash,
    string Salt,
    bool IsVerified,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    bool IsDeleted
);