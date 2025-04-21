using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.Candidates.Queries;

public record ExamWithResultResponse(ExamResponse Exam, ResultResponse? Result);

public record GetAllExamsByCandidateQuery : IRequest<ErrorOr<List<ExamWithResultResponse>>>;

public class GetAllExamsByCandidateQueryHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<GetAllExamsByCandidateQuery, ErrorOr<List<ExamWithResultResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<List<ExamWithResultResponse>>> Handle(
        GetAllExamsByCandidateQuery request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();
        var exams = await _unitOfWork.Exam.GetByAccountIdAsync(userAccountId, cancellationToken);

        return exams.Select(e =>
            new ExamWithResultResponse(
                e.MapToDto(),
                e.ExamCandidates.FirstOrDefault()?.MapToResultDto()
            )
        ).ToList();
    }
}