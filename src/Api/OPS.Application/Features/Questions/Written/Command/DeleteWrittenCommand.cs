using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Written.Command;

public record DeleteWrittenCommand(Guid QuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteWrittenCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteWrittenCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteWrittenCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWrittenByIdAsync(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();
        
        _unitOfWork.Question.Remove(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);
        
        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteWrittenCommandValidator : AbstractValidator<DeleteWrittenCommand>
{
    public DeleteWrittenCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}