using OPS.Domain.Enums;

namespace OPS.Application.Dtos;

public record AccountResponse(
    Guid AccountId,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);

public record AccountBasicInfoResponse(
    Guid AccountId,
    string Username,
    string Email
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
    CloudFileResponse? ImageFile,
    List<ProfileLinkRequest> ProfileLinks
);

public record ProfileLinkRequest(
    Guid? ProfileLinkId,
    string? Name,
    string? Link
);

public record AuthenticationResponse(
    string Token,
    AccountWithDetailsResponse Account
);