using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Submit;
using OPS.Domain;

namespace OPS.Application.Features.WrittenSubmissions.Commands;

public record UpdateWrittenSubmissionCommand(
    Guid WrittenSubmissionId,
    string? Answer,
    decimal? Score) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class UpdateWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWrittenSubmissionCommand, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(UpdateWrittenSubmissionCommand command,
        CancellationToken cancellationToken)
    {
        var writtenSubmission = await _unitOfWork.WrittenSubmission.GetAsync(command.WrittenSubmissionId, cancellationToken);

        if (writtenSubmission is null) return Error.NotFound("WrittenSubmission was not found");

        writtenSubmission.Answer = command.Answer ?? writtenSubmission.Answer;
        writtenSubmission.Score = command.Score ?? writtenSubmission.Score;
        writtenSubmission.UpdatedAt = DateTime.UtcNow;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? writtenSubmission.ToDto()
            : Error.Failure("The WrittenSubmission could not be saved.");
    }
}

public class UpdateWrittenSubmissionCommandValidator : AbstractValidator<UpdateWrittenSubmissionCommand>
{
    public UpdateWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.WrittenSubmissionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.Answer)
            .NotEmpty()
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Answer));

        RuleFor(x => x.Score)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .When(x => x.Score.HasValue);
    }
}