using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Queries;

public record GetAllProblemSolvingByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<ProblemQuestionResponse>>>;

public class GetAllProblemSolvingByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProblemSolvingByExamIdQuery, ErrorOr<List<ProblemQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuestionResponse>>> Handle(GetAllProblemSolvingByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetProblemSolvingByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(q => q.ToProblemQuestionDto()).ToList();
    }
}

public class GetAllProblemSolvingByExamIdQueryValidator : AbstractValidator<GetAllProblemSolvingByExamIdQuery>
{
    public GetAllProblemSolvingByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}