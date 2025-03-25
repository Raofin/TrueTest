using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.Examinations.Queries;

public record GetExamsByUserQuery: IRequest<ErrorOr<List<ExamResponse>>>;

public class GetExamsByAccountIdQueryHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<GetExamsByUserQuery, ErrorOr<List<ExamResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<List<ExamResponse>>> Handle(GetExamsByUserQuery request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var exams = await _unitOfWork.Exam.GetByAccountIdAsync(userAccountId, cancellationToken);

        return exams.Select(e => e.ToDto()).ToList();
    }
}