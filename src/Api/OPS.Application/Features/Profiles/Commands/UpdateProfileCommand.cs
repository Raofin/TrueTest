using Azure.Core;
using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Features.Profiles.Commands;

public record UpdateProfileCommand(
    Guid ProfileId,
    string FirstName,
    string LastName,
    string BioMarkdown,
    string InstituteName,
    string PhoneNumber,
    Guid? ImageFileId,
    List<ProfileSocialResponse> ProfileSocials

    ) : IRequest<ErrorOr<ProfileResponse>>;


public class UpdateProfileCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProfileCommand, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProfileResponse>> Handle(UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profile.GetAsync(command.ProfileId, cancellationToken);
        if (profile is null) return Error.NotFound();

        profile.FirstName = command.FirstName ?? command.FirstName;
        profile.LastName = command.LastName ?? command.LastName;
        profile.BioMarkdown = command.BioMarkdown ?? command.BioMarkdown;
        profile.InstituteName = command.InstituteName ?? command.InstituteName;
        profile.PhoneNumber = command.PhoneNumber ?? command.PhoneNumber;
        profile.ImageFileId = command.ImageFileId == Guid.Empty ? null : command.ImageFileId;
        profile.ImageFile = null;
        profile.AccountId = Guid.Parse("41FA5C6E-AC17-4C63-9BED-AF6FECE20990");


        if (command.ProfileSocials != null)
        {
            foreach (var profileSocial in command.ProfileSocials)
            {
                if (profileSocial.Id != Guid.Empty)
                {
                    var social = await _unitOfWork.ProfileSocial.GetAsync(profileSocial.Id, cancellationToken);
                    if (social is null) return Error.NotFound();
                    social.Name = profileSocial.Name ?? profileSocial.Name;
                    social.Link = profileSocial.Link ?? profileSocial.Link;
                }
                else
                {

                    var social = new ProfileSocial
                    {
                        ProfileId = profile.Id,
                        Name = profileSocial.Name,
                        Link = profileSocial.Link
                    };
                    _unitOfWork.ProfileSocial.Add(social);
                }
            }
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? profile.ToDto()
            : Error.Failure();
    }
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}