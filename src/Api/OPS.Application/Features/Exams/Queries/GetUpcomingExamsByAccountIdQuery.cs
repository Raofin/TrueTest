using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetUpcomingExamsByAccountIdQuery(Guid AccountId) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetUpcomingExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUpcomingExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetUpcomingExamsByAccountIdQuery
        request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetUpcomingExamsByAccountIdAsync(request.AccountId, cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}