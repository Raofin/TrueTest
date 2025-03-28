using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record GetProblemQuesWithSubmissionQuery(Guid ExamId, Guid AccountId)
    : IRequest<ErrorOr<List<ProblemQuesWithSubmissionResponse?>>>;

public class GetProblemQuesWithSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemQuesWithSubmissionQuery, ErrorOr<List<ProblemQuesWithSubmissionResponse?>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuesWithSubmissionResponse?>>> Handle(
        GetProblemQuesWithSubmissionQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.ProblemSubmission
            .GetAllProblemsWithSubmission(request.ExamId, request.AccountId, cancellationToken);

        return questions.Select(q => q.ToProblemWithSubmissionDto()).ToList();
    }
}

public class GetProblemQuesWithSubmissionQueryValidator : AbstractValidator<GetProblemQuesWithSubmissionQuery>
{
    public GetProblemQuesWithSubmissionQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}