using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetUpcomingExamsByAccountIdQuery(Guid Id) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetUpcomingExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUpcomingExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetUpcomingExamsByAccountIdQuery
        request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetUpcomingExamsByAccountIdAsync(request.Id, cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}