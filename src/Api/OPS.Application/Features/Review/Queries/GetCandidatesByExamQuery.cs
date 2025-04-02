using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record GetCandidatesByExamQuery(Guid ExamId) : IRequest<ErrorOr<List<ExamCandidatesResponse>>>;

public class GetCandidatesByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCandidatesByExamQuery, ErrorOr<List<ExamCandidatesResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamCandidatesResponse>>> Handle(
        GetCandidatesByExamQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _unitOfWork.ExamCandidate.GetExamParticipantsAsync(request.ExamId, cancellationToken);
        
        return candidates.Select(
            candidate => new ExamCandidatesResponse(
                candidate.Account!.Id,
                candidate.Account.Username,
                candidate.Account.Email,
                candidate.Score,
                "status", // TODO: Add column in DB
                candidate.StartedAt!.Value,
                candidate.SubmittedAt!.Value
            )
        ).ToList();
    }
}