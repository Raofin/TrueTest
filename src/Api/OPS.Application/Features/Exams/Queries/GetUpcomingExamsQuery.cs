using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetUpcomingExams : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetUpcomingExamsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUpcomingExams, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetUpcomingExams request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetUpcomingExamsAsync(cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}