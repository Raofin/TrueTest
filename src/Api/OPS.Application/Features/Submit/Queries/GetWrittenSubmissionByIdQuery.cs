using MediatR;
using ErrorOr;
using OPS.Application.Contracts.Submit;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Submit.Queries;

public record GetWrittenSubmissionByIdQuery(Guid writtenSubmissionId) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class GetWrittenSubmissionByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetWrittenSubmissionByIdQuery, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(GetWrittenSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var WrittenSubmission = await _unitOfWork.WrittenSubmission.GetAsync(request.writtenSubmissionId, cancellationToken);

        return WrittenSubmission is null
            ? Error.NotFound("WrittenSubmission not found.")
            : WrittenSubmission.ToDto();
    }
}