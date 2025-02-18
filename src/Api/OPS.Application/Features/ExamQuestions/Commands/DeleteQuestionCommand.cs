using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Commands;

public record DeleteQuestionCommand(Guid QuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteQuestionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteQuestionCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);

        if (question is null) return Error.NotFound();

        _unitOfWork.Question.Remove(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}