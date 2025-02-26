using ErrorOr;
using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Application.Contracts.DtoExtensions;

public static class ProfileSocialExtensions
{
    public static ProfileSocialResponse ToDto(this ProfileSocial profileSocial)
    {
        return new ProfileSocialResponse(
            profileSocial.Id,
            profileSocial.Name,
            profileSocial.Link
            );
    }
}