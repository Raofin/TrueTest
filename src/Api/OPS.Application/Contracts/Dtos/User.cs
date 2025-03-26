using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record AccountResponse(
    Guid AccountId,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);

public record AuthenticationResponse(
    string Token,
    AccountWithDetailsResponse AccountWithDetails
);

public record AccountWithDetailsResponse(
    Guid AccountId,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    List<RoleType> Roles,
    ProfileResponse? Profile
);

public record ProfileResponse(
    Guid ProfileId,
    string? FirstName,
    string? LastName,
    string? BioMarkdown,
    string? InstituteName,
    string? PhoneNumber,
    Guid? ImageFileId,
    List<ProfileLinkRequest> ProfileLinks
);

public record ProfileLinkRequest(
    Guid? Id,
    string? Name,
    string? Link
);