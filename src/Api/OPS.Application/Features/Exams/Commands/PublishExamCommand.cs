using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record PublishExamCommand(Guid ExamId) : IRequest<ErrorOr<Success>>;

public class PublishExamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<PublishExamCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(PublishExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuestionsAsync(request.ExamId, cancellationToken);
        if (exam is null) return Error.NotFound();
        
        if (exam.TotalPoints != exam.Questions.Sum(q => q.Points))
            return Error.Conflict(description: "Total points of questions do not match the exam total points.");
        
        if (exam.IsPublished) return Error.Conflict(description: "Exam is already published");

        exam.IsPublished = true;

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class PublishExamCommandValidator : AbstractValidator<PublishExamCommand>
{
    public PublishExamCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}