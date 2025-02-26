using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.User;

namespace OPS.Application.Contracts.Dtos;

public record ProfileSocialResponse(
    Guid Id,
    string Name,
    string Link
);