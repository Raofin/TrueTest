using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.McqQuestions.Commands;

public record DeleteMcqQuestionCommand(Guid McqQuestionId) : IRequest<ErrorOr<Success>>;

public class DeleteMcqQuestionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMcqQuestionCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteMcqQuestionCommand request, CancellationToken cancellationToken)
    {
        var mcqQuestion = await _unitOfWork.Question.GetAsync(request.McqQuestionId, cancellationToken);

        if (mcqQuestion is null) return Error.NotFound();

        var options = await _unitOfWork.McqOption.GetMcqOptionsByQuestionIdAsync(mcqQuestion.Id, cancellationToken);

        foreach (var option in options)
        {
            var mcqSubmissions = await _unitOfWork.McqSubmission.GetMcqSubmissionsByMcqOptionIdAsync(option.Id, cancellationToken);
            _unitOfWork.McqSubmission.RemoveRange(mcqSubmissions);
        }   

        _unitOfWork.McqOption.RemoveRange(options); 
        _unitOfWork.Question.Remove(mcqQuestion);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure();
    }
}

public class DeleteMcqQuestionCommandValidator : AbstractValidator<DeleteMcqQuestionCommand>
{
    public DeleteMcqQuestionCommandValidator()
    {
        RuleFor(x => x.McqQuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}