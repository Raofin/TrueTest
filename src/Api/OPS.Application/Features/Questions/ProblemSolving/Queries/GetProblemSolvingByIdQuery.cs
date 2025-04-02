using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Queries;

public record GetProblemSolvingByIdQuery(Guid Id) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class GetProblemSolvingByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSolvingByIdQuery, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(GetProblemSolvingByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithTestCases(request.Id, cancellationToken);

        return question is null
            ? Error.NotFound()
            : question.ToProblemQuestionDto();
    }
}

public class GetProblemSolvingByIdQueryValidator : AbstractValidator<GetProblemSolvingByIdQuery>
{
    public GetProblemSolvingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}