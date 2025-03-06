using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.ProblemQuestions.Commands;

public record DeleteProblemQuestionCommand(Guid ProblemQuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteProblemQuestionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProblemQuestionCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteProblemQuestionCommand request, CancellationToken cancellationToken)
    {
        var problemQuestion = await _unitOfWork.Question.GetAsync(request.ProblemQuestionId, cancellationToken);

        if (problemQuestion is null) return Error.NotFound();

        var TestCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(problemQuestion.Id, cancellationToken);

        foreach (var testCase in TestCases)
        {
            _unitOfWork.TestCase.Remove(testCase);
        }   

        _unitOfWork.Question.Remove(problemQuestion);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteProblemQuestionCommandValidator : AbstractValidator<DeleteProblemQuestionCommand>
{
    public DeleteProblemQuestionCommandValidator()
    {
        RuleFor(x => x.ProblemQuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}