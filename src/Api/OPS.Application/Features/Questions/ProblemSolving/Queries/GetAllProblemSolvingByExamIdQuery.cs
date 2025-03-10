using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Queries;

public record GetProblemSolvingByExamQuery(Guid ExamId) : IRequest<ErrorOr<List<ProblemQuestionResponse>>>;

public class GetProblemSolvingByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSolvingByExamQuery, ErrorOr<List<ProblemQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuestionResponse>>> Handle(GetProblemSolvingByExamQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetProblemSolvingByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(q => q.ToProblemQuestionDto()).ToList();
    }
}

public class GetProblemSolvingByExamQueryValidator : AbstractValidator<GetProblemSolvingByExamQuery>
{
    public GetProblemSolvingByExamQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}