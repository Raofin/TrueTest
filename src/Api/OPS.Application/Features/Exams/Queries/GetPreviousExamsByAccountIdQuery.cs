using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetPreviousExamsByAccountIdQuery(Guid AccountId) : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetPreviousExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPreviousExamsByAccountIdQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetPreviousExamsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetPreviousExamsByAccountIdAsync(request.AccountId, cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}