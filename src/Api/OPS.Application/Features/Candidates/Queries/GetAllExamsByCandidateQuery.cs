using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.Candidates.Queries;

public record GetAllExamsByCandidateQuery : IRequest<ErrorOr<List<ExamResponse>>>;

public class GetAllExamsByCandidateQueryHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<GetAllExamsByCandidateQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(
        GetAllExamsByCandidateQuery request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();
        var exams = await _unitOfWork.Exam.GetByAccountIdAsync(userAccountId, cancellationToken);
        return exams.Select(e => e.ToDto()).ToList();
    }
}