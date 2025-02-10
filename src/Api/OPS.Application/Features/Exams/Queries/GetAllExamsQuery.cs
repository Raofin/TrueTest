using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetAllExamsQuery : IRequest<ErrorOr<List<ProfileResponse>>>;

public class GetAllExamsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllExamsQuery, ErrorOr<List<ProfileResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProfileResponse>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
        var exams = await _unitOfWork.Exam.GetAsync(cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}