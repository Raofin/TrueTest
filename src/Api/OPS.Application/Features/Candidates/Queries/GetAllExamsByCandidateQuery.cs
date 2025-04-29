using System.Diagnostics.CodeAnalysis;
using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Candidates.Queries;

public record ExamWithResultResponse(ExamResponse Exam, ResultResponse? Result);

[ExcludeFromCodeCoverage]
public record GetAllExamsByCandidateQuery : IRequest<ErrorOr<List<ExamWithResultResponse>>>;

public class GetAllExamsByCandidateQueryHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
    : IRequestHandler<GetAllExamsByCandidateQuery, ErrorOr<List<ExamWithResultResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;

    public async Task<ErrorOr<List<ExamWithResultResponse>>> Handle(
        GetAllExamsByCandidateQuery request, CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();
        var exams = await _unitOfWork.Exam.GetByAccountIdAsync(userAccountId, cancellationToken);

        return exams.Select(e =>
            new ExamWithResultResponse(
                e.MapToDto(),
                e.ExamCandidates.FirstOrDefault()?.MapToResultDto()
            )
        ).ToList();
    }
}