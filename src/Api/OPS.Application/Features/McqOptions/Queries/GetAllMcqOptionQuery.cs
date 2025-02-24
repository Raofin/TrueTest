using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.McqOptions.Queries;

public record GetAllMcqOptionQuery : IRequest<ErrorOr<List<McqOptionResponse>>>;

public class GetAllMcqOptionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllMcqOptionQuery, ErrorOr<List<McqOptionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqOptionResponse>>> Handle(
        GetAllMcqOptionQuery request, CancellationToken cancellationToken)
    {
        var mcqOptions = await _unitOfWork.McqOption.GetAsync(cancellationToken);

        return mcqOptions.Select(e => e.ToDto()).ToList();
    }
}