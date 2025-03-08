using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Mcq.Commands;

public record DeleteMcqCommand(Guid QuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMcqCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteMcqCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithMcqOption(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        if (question.McqOption is not null)
            _unitOfWork.McqOption.Remove(question.McqOption);

        _unitOfWork.Question.Remove(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteMcqCommandValidator : AbstractValidator<DeleteMcqCommand>
{
    public DeleteMcqCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}