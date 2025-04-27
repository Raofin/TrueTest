using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.CloudFiles.Queries;

public record GetFileDetailsQuery(Guid CloudFileId) : IRequest<ErrorOr<CloudFileResponse>>;

public class GetFileDetailsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFileDetailsQuery, ErrorOr<CloudFileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<CloudFileResponse>> Handle(GetFileDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var fileInfo = await _unitOfWork.CloudFile.GetAsync(request.CloudFileId, cancellationToken);

        return fileInfo is null
            ? Error.NotFound()
            : fileInfo.MapToDto();
    }
}

public class GetFileDetailsQueryValidator : AbstractValidator<GetFileDetailsQuery>
{
    public GetFileDetailsQueryValidator()
    {
        RuleFor(x => x.CloudFileId).NotEmpty();
    }
}