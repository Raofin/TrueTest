using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.McqSubmissions.Commands;

public record UpdateMcqQuestionCommand(
    Guid McqSubmissionId,
    string AnswerOptions
    ) : IRequest<ErrorOr<McqSubmissionResponse>>;

public class UpdateMcqSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMcqQuestionCommand, ErrorOr<McqSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqSubmissionResponse>> Handle(UpdateMcqQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var mcqSubmission = await _unitOfWork.McqSubmission.GetAsync(command.McqSubmissionId, cancellationToken);

        if (mcqSubmission is null) return Error.NotFound(); 

        mcqSubmission.AnswerOptions = command.AnswerOptions ?? mcqSubmission.AnswerOptions;  
        mcqSubmission.UpdatedAt = DateTime.UtcNow;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? mcqSubmission.ToDto()
            : Error.Failure();
    }
}

public class UpdateMcqSubmissionCommandValidator : AbstractValidator<UpdateMcqQuestionCommand>
{
    public UpdateMcqSubmissionCommandValidator()
    {
        RuleFor(x => x.McqSubmissionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.AnswerOptions)
            .NotEmpty();
    }
}