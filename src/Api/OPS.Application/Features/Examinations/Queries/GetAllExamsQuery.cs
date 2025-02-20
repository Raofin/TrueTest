using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Examinations.Queries;

public record GetAllExamsQuery : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetAllExamsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllExamsQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetAsync(cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}