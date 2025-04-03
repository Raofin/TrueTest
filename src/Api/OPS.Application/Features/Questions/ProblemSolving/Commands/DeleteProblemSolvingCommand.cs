using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record DeleteProblemSolvingCommand(Guid QuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteProblemSolvingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProblemSolvingCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteProblemSolvingCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithTestCases(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        question.Examination.ProblemSolvingPoints -= question.Points;
        question.Examination.TotalPoints -= question.Points;

        _unitOfWork.TestCase.RemoveRange(question.TestCases);
        _unitOfWork.Question.Remove(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteProblemSolvingCommandValidator : AbstractValidator<DeleteProblemSolvingCommand>
{
    public DeleteProblemSolvingCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}