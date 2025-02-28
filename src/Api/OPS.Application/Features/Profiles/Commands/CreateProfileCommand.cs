using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Features.Profiles.Commands;

public record CreateProfileCommand(
            string FirstName,
            string LastName,
            string BioMarkdown,
            string InstituteName,
            string PhoneNumber,
            Guid AccountId,
            Guid ImageFileId,
            List<ProfileSocialResponse> ProfileSocials
    ) : IRequest<ErrorOr<ProfileResponse>>;

public class CreateProfileCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProfileCommand, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProfileResponse>> Handle(CreateProfileCommand request,
        CancellationToken cancellationToken)
    {
       // var accountExists = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
      //  if (accountExists == null) return Error.NotFound();   

        var profile = new Profile
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            BioMarkdown = request.BioMarkdown,
            InstituteName = request.InstituteName,
            PhoneNumber = request.PhoneNumber,  
            AccountId = Guid.Parse("41FA5C6E-AC17-4C63-9BED-AF6FECE20990"),
            ImageFileId = null,
            ImageFile = null
        };

        _unitOfWork.Profile.Add(profile); 

        if(request.ProfileSocials != null)
        {
            foreach (var profileSocial in request.ProfileSocials)
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

        var result = await _unitOfWork.CommitAsync(cancellationToken);
        return result > 0
            ? profile.ToDto()
            : Error.Failure();


    }
}

public class CreateProfilenCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfilenCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();


        RuleFor(x => x.AccountId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}