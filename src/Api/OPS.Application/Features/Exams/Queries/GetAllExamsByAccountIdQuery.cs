using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetAllExamsByAccountIdQuery(Guid AccountId) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetAllExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetAllExamsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetAllExamsByAccountIdAsync(request.AccountId, cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}