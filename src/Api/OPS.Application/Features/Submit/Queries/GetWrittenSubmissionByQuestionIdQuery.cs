using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Submit;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Submit.Queries;

public record GetWrittenSubmissionByQuestionIdQuery(Guid questionId) : IRequest<ErrorOr<List<WrittenSubmissionResponse>>>;

public class GetWrittenSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenSubmissionByQuestionIdQuery, ErrorOr<List<WrittenSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenSubmissionResponse>>> Handle(GetWrittenSubmissionByQuestionIdQuery request, CancellationToken cancellationToken)
    {
        var WrittenSubmissions = await _unitOfWork.WrittenSubmission.GetAllWrittenSubmissionByQuestionIdAsync(request.questionId, cancellationToken);

        return WrittenSubmissions.Select(e => e.ToDto()).ToList();
    }
}