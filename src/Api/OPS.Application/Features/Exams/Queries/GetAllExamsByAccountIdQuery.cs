using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetAllExamsByAccountIdQuery(Guid Id) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetAllExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetAllExamsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetAllExamsByAccountIdAsync(request.Id, cancellationToken);
        return exams.Select(e => e.ToDto()).ToList();
    }
}