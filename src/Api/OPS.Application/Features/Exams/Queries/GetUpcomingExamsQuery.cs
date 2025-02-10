using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetUpcomingExams : IRequest<ErrorOr<List<ProfileResponse>>>;

public class GetUpcomingExamsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUpcomingExams, ErrorOr<List<ProfileResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProfileResponse>>> Handle(GetUpcomingExams request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetUpcomingExamsAsync(cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}