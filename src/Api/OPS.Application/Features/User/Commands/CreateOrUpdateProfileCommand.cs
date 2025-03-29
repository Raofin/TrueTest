using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;
using Throw;

namespace OPS.Application.Features.User.Commands;

public record CreateOrUpdateProfileCommand(
    string? FirstName,
    string? LastName,
    string? Bio,
    string? InstituteName,
    string? PhoneNumber,
    Guid? ImageFileId,
    List<ProfileLinkRequest> ProfileLinks) : IRequest<ErrorOr<ProfileResponse>>;

public class CreateOrUpdateProfileCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<CreateOrUpdateProfileCommand, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<ProfileResponse>> Handle(CreateOrUpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        if (request.ImageFileId.HasValue)
        {
            var isExists = await _unitOfWork.CloudFile.IsExistsAsync(request.ImageFileId.Value, cancellationToken);
            if (!isExists) throw new NullReferenceException("Image file does not exist.");
        }

        var profile = await _unitOfWork.Profile.GetByAccountId(userAccountId, cancellationToken);

        if (profile is null)
        {
            profile = new Profile
            {
                AccountId = userAccountId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Bio = request.Bio,
                InstituteName = request.InstituteName,
                PhoneNumber = request.PhoneNumber,
                ImageFileId = request.ImageFileId,
                ProfileLinks = request.ProfileLinks
                    .Select(x => new ProfileLinks { Name = x.Name!, Link = x.Link! })
                    .ToList()
            };

            _unitOfWork.Profile.Add(profile);
        }
        else
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

            foreach (var linkRequest in request.ProfileLinks)
            {
                if (!linkRequest.ProfileLinkId.HasValue)
                {
                    var profileLink = new ProfileLinks
                    {
                        ProfileId = profile.Id,
                        Name = linkRequest.Name!,
                        Link = linkRequest.Link!
                    };

                    _unitOfWork.ProfileLink.Add(profileLink);
                }
                else
                {
                    var profileLink = await _unitOfWork.ProfileLink.GetAsync(
                        linkRequest.ProfileLinkId!.Value, cancellationToken);
                    profileLink.ThrowIfNull("Profile link not found.");

                    profileLink.Name = linkRequest.Name ?? profileLink.Name;
                    profileLink.Link = linkRequest.Link ?? profileLink.Link;
                }
            }
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        if (result <= 0) return Error.Failure();
        profile = await _unitOfWork.Profile.GetByAccountId(userAccountId, cancellationToken);

        return profile.ToDto()!;
    }
}

public class CreateProfileCommandValidator : AbstractValidator<CreateOrUpdateProfileCommand>
{
    public CreateProfileCommandValidator()
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