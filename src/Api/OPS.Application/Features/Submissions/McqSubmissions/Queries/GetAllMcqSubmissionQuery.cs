using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Submissions.McqSubmissions.Queries;

public record GetAllMcqSubmissionQuery : IRequest<ErrorOr<List<McqSubmissionResponse>>>;

public class GetAllMcqSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllMcqSubmissionQuery, ErrorOr<List<McqSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqSubmissionResponse>>> Handle(
        GetAllMcqSubmissionQuery request, CancellationToken cancellationToken)
    {
        var mcqSubmissions = await _unitOfWork.McqSubmission.GetAsync(cancellationToken);

        // return mcqSubmissions.Select(e => e.ToDto()).ToList();
        return Error.Failure();

    }
}