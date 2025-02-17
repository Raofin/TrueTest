using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Submit;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Commands;

public record UpdateWrittenSubmissionCommand(
    Guid Id,
    string? Answer,
    decimal? Score
) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class UpdateWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWrittenSubmissionCommand, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(UpdateWrittenSubmissionCommand command,
        CancellationToken cancellationToken)
    {
        var WrittenSubmission = await _unitOfWork.WrittenSubmission.GetAsync(command.Id, cancellationToken);

        if (WrittenSubmission is null) return Error.NotFound("WrittenSubmission was not found");

        WrittenSubmission.Answer = command.Answer ?? WrittenSubmission.Answer;
        WrittenSubmission.Score = command.Score ?? WrittenSubmission.Score;
        WrittenSubmission.UpdatedAt = DateTime.UtcNow;


        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? WrittenSubmission.ToDto()
            : Error.Failure("The WrittenSubmission could not be saved.");
    }
}

public class UpdateWrittenSubmissionCommandValidator : AbstractValidator<UpdateWrittenSubmissionCommand>
{
    public UpdateWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("Answer is required.")
            .MaximumLength(5000).WithMessage("Answer cannot exceed 5000 characters.");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");
    }
}