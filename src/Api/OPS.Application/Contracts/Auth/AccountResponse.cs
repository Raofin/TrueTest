namespace OPS.Application.Contracts.Auth;

public record AccountResponse(
    long AccountId,
    string Username,
    string Email,
    string PasswordHash,    
    string Salt,    
    bool isVerified,  
    long? CloudFileId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    bool IsDeleted
);