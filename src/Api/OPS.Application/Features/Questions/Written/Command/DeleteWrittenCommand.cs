using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Written.Command;

public record DeleteWrittenCommand(Guid QuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteWrittenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteWrittenCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteWrittenCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithExamAsync(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        if (question.Examination.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        question.Examination.WrittenPoints -= question.Points;

        _unitOfWork.Question.Remove(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class DeleteWrittenCommandValidator : AbstractValidator<DeleteWrittenCommand>
{
    public DeleteWrittenCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .IsValidGuid();
    }
}