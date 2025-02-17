using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Submit;
using OPS.Domain;

namespace OPS.Application.Features.WrittenSubmissions.Queries;

public record GetWrittenSubmissionByQuestionIdQuery(Guid QuestionId) : IRequest<ErrorOr<List<WrittenSubmissionResponse>>>;

public class GetWrittenSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenSubmissionByQuestionIdQuery, ErrorOr<List<WrittenSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenSubmissionResponse>>> Handle(GetWrittenSubmissionByQuestionIdQuery request,
        CancellationToken cancellationToken)
    {
        var writtenSubmissions = await _unitOfWork.WrittenSubmission.GetByQuestionIdAsync(request.QuestionId, cancellationToken);

        return writtenSubmissions.Select(e => e.ToDto()).ToList();
    }
}