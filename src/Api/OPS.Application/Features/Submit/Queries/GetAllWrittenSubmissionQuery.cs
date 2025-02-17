using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Submit;
using OPS.Domain;

namespace OPS.Application.Features.Submit.Queries;

public record GetAllWrittenSubmissionQuery : IRequest<ErrorOr<List<WrittenSubmissionResponse>>>;

public class GetAllWrittenSubmissionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllWrittenSubmissionQuery, ErrorOr<List<WrittenSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenSubmissionResponse>>> Handle(
        GetAllWrittenSubmissionQuery request, CancellationToken cancellationToken)
    {
        var writtenSubmissions = await _unitOfWork.WrittenSubmission.GetAsync(cancellationToken);

        return writtenSubmissions.Select(e => e.ToDto()).ToList();
    }
}