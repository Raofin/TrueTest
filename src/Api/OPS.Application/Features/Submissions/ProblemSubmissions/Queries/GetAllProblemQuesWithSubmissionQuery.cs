using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Submissions.ProblemSubmissions.Queries;

public record GetAllProblemQuesWithSubmissionQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<List<ProblemQuesWithSubmissionResponse>>>;

public class GetAllProblemQuesWithSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProblemQuesWithSubmissionQuery, ErrorOr<List<ProblemQuesWithSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuesWithSubmissionResponse>>> Handle(
        GetAllProblemQuesWithSubmissionQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.ProblemSubmission
            .GetAllProblemsWithSubmission(request.ExamId, request.AccountId, cancellationToken);

        return questions.Select(q => q.ToQuesProblemSubmissionDto()).ToList();
    }
}

public class GetAllProblemQuesWithSubmissionQueryValidator : AbstractValidator<GetAllProblemQuesWithSubmissionQuery>
{
    public GetAllProblemQuesWithSubmissionQueryValidator()
    {
        RuleFor(x => x.ExamId).NotEqual(Guid.Empty);
    }
}