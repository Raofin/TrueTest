using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;

namespace OPS.Application.Features.User.Commands;

public record SaveProfileCommand(
    string? FirstName,
    string? LastName,
    string? Bio,
    string? InstituteName,
    string? PhoneNumber,
    Guid? ImageFileId,
    List<ProfileLinkRequest> ProfileLinks) : IRequest<ErrorOr<ProfileResponse>>;

public class SaveProfileCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveProfileCommand, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<ProfileResponse>> Handle(SaveProfileCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        if (!await ValidateImageFile(request.ImageFileId, cancellationToken))
            return Error.Unexpected(description: "Image file not found.");

        var profile = await _unitOfWork.Profile.GetByAccountId(userAccountId, cancellationToken);

        if (profile is null)
        {
            CreateProfile(request, userAccountId);
        }
        else
        {
            await UpdateProfile(profile, request, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        profile = await _unitOfWork.Profile.GetByAccountId(userAccountId, cancellationToken);
        return profile.MapToDto()!;
    }

    private async Task<bool> ValidateImageFile(Guid? imageFileId, CancellationToken cancellationToken)
    {
        if (!imageFileId.HasValue) return true;
        return await _unitOfWork.CloudFile.IsExistsAsync(imageFileId.Value, cancellationToken);
    }

    private void CreateProfile(SaveProfileCommand request, Guid accountId)
    {
        var profile = new Profile
        {
            AccountId = accountId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Bio = request.Bio,
            InstituteName = request.InstituteName,
            PhoneNumber = request.PhoneNumber,
            ImageFileId = request.ImageFileId,
            ProfileLinks = request.ProfileLinks.Select(
                x => new ProfileLinks
                {
                    Name = x.Name!,
                    Link = x.Link!
                }
            ).ToList()
        };

        _unitOfWork.Profile.Add(profile);
    }

    private async Task UpdateProfile(Profile profile, SaveProfileCommand request, CancellationToken cancellationToken)
    {
        profile.FirstName = request.FirstName ?? profile.FirstName;
        profile.LastName = request.LastName ?? profile.LastName;
        profile.Bio = request.Bio ?? profile.Bio;
        profile.InstituteName = request.InstituteName ?? profile.InstituteName;
        profile.PhoneNumber = request.PhoneNumber ?? profile.PhoneNumber;

        if (request.ImageFileId.HasValue)
        {
            profile.ImageFileId = request.ImageFileId;
        }

        await UpdateProfileLinks(profile, request.ProfileLinks, cancellationToken);
    }

    private async Task UpdateProfileLinks(Profile profile, List<ProfileLinkRequest> links,
        CancellationToken cancellationToken)
    {
        foreach (var linkRequest in links)
        {
            if (!linkRequest.ProfileLinkId.HasValue)
            {
                _unitOfWork.ProfileLink.Add(new ProfileLinks
                {
                    ProfileId = profile.Id,
                    Name = linkRequest.Name!,
                    Link = linkRequest.Link!
                });
            }
            else
            {
                var profileLink =
                    await _unitOfWork.ProfileLink.GetAsync(linkRequest.ProfileLinkId.Value, cancellationToken);
                if (profileLink is null) continue;

                profileLink.Name = linkRequest.Name ?? profileLink.Name;
                profileLink.Link = linkRequest.Link ?? profileLink.Link;
            }
        }
    }
}

public class SaveProfileCommandValidator : AbstractValidator<SaveProfileCommand>
{
    public SaveProfileCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .MaximumLength(20)
            .When(command => !string.IsNullOrEmpty(command.FirstName));

        RuleFor(command => command.LastName)
            .MaximumLength(20)
            .When(command => !string.IsNullOrEmpty(command.LastName));

        RuleFor(command => command.Bio)
            .MaximumLength(200)
            .When(command => !string.IsNullOrEmpty(command.Bio));

        RuleFor(command => command.InstituteName)
            .MaximumLength(50)
            .When(command => !string.IsNullOrEmpty(command.InstituteName));

        RuleFor(command => command.PhoneNumber)
            .MaximumLength(20)
            .When(command => !string.IsNullOrEmpty(command.PhoneNumber));

        RuleForEach(command => command.ProfileLinks)
            .SetValidator(new ProfileLinkRequestValidator());
    }
}

public class ProfileLinkRequestValidator : AbstractValidator<ProfileLinkRequest>
{
    public ProfileLinkRequestValidator()
    {
        RuleFor(x => x.ProfileLinkId)
            .Must(x => !x.HasValue || x.Value != Guid.Empty)
            .When(x => x.ProfileLinkId.HasValue);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Link)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Link));
    }
}