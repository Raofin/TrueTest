using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.McqSubmissions.Queries;

public record GetAllMcqSubmissionQuery : IRequest<ErrorOr<List<McqSubmissionResponse>>>;

public class GetAllMcqSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllMcqSubmissionQuery, ErrorOr<List<McqSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqSubmissionResponse>>> Handle(
        GetAllMcqSubmissionQuery request, CancellationToken cancellationToken)
    {
        var mcqSubmissions = await _unitOfWork.McqSubmission.GetAsync(cancellationToken);

        return mcqSubmissions.Select(e => e.ToDto()).ToList();
    }
}