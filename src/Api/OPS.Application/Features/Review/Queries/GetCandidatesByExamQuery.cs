using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record CandidateResultResponse(AccountBasicInfoResponse Account, ResultResponse? Result);

public record ExamResultsResponse(ExamResponse Exam, List<CandidateResultResponse> Candidates);

public record GetCandidatesByExamQuery(Guid ExamId) : IRequest<ErrorOr<ExamResultsResponse>>;

public class GetCandidatesByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCandidatesByExamQuery, ErrorOr<ExamResultsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResultsResponse>> Handle(
        GetCandidatesByExamQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetResultsAsync(request.ExamId, cancellationToken);

        if (exam is null) return Error.NotFound();

        return new ExamResultsResponse(
            exam.MapToDto(),
            exam.ExamCandidates.Select(
                candidate => new CandidateResultResponse(
                    candidate.Account!.MapToBasicInfoDto(),
                    candidate.MapToResultDto()
                )
            ).ToList()
        );
    }
}