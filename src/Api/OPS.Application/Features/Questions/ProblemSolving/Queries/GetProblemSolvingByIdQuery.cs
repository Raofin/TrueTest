using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Queries;

public record GetProblemSolvingByIdQuery(Guid QuestionId) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class GetProblemSolvingByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSolvingByIdQuery, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(GetProblemSolvingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithTestCases(request.QuestionId, cancellationToken);

        return question is null
            ? Error.NotFound()
            : question.MapToProblemQuestionDto();
    }
}

public class GetProblemSolvingByIdQueryValidator : AbstractValidator<GetProblemSolvingByIdQuery>
{
    public GetProblemSolvingByIdQueryValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}