using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.User;

namespace OPS.Application.Contracts.Dtos;

public record ProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string BioMarkdown,
    string InstituteName,
    string PhoneNumber,
    Guid AccountId, 
    Guid? ImageFileId,
    CloudFile? ImageFile,
    ICollection<ProfileSocial> ProfileSocials
); 