using ErrorOr;
using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Application.Contracts.DtoExtensions;

public static class ProfileExtensions
{
    public static ProfileResponse ToDto(this Profile profile)
    {
        return new ProfileResponse(
            profile.Id,
            profile.FirstName,
            profile.LastName,
            profile.BioMarkdown,
            profile.InstituteName,
            profile.PhoneNumber,
            profile.AccountId,
            profile.ImageFileId,
            profile.ImageFile,
            profile.ProfileSocials
            );
    }
}