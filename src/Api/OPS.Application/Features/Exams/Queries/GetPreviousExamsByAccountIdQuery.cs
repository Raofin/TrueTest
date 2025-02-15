using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetPreviousExamsByAccountIdQuery(Guid Id) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetPreviousExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPreviousExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetPreviousExamsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetPreviousExamsByAccountIdAsync(request.Id, cancellationToken);
        return exams.Select(e => e.ToDto()).ToList();
    }
}