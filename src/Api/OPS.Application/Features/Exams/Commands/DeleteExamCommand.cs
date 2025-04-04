using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record DeleteExamCommand(Guid ExamId) : IRequest<ErrorOr<Success>>;

public class DeleteExamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteExamCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);

        if (exam is null) return Error.NotFound();
        if (exam.IsPublished) return Error.Validation("Exam is already published");

        _unitOfWork.Exam.Remove(exam);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class DeleteExamCommandValidator : AbstractValidator<DeleteExamCommand>
{
    public DeleteExamCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}