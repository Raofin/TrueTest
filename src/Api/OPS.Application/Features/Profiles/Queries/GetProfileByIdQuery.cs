using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;


namespace OPS.Application.Features.Profiles.Queries;

public record GetProfileByIdQuery(Guid ProfileId) : IRequest<ErrorOr<ProfileResponse>>;

public class GetProfileByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProfileByIdQuery, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProfileResponse>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profile.GetAsync(request.ProfileId, cancellationToken);
        if(profile == null) 
            return Error.NotFound();    

        var socials = await _unitOfWork.ProfileSocial.GetProfileSocialsByProfileIdAsync(request.ProfileId, cancellationToken);

        profile.ProfileSocials = socials.ToList();

        return profile is null
            ? Error.NotFound()
            : profile.ToDto();
    }
}

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}