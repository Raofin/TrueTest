using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.ProblemSubmissions.Commands;

public record UpdateProblemSubmitCommand(
    Guid Id,
    string Code,
    int Attempts,
    decimal Score,
    bool IsFlagged,
    string? FlagReason,
    int ProgLanguageId
    ) : IRequest<ErrorOr<ProblemSubmitResponse>>;

public class UpdateProblemSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProblemSubmitCommand, ErrorOr<ProblemSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemSubmitResponse>> Handle(UpdateProblemSubmitCommand command,
        CancellationToken cancellationToken)
    {
        var problemSubmission = await _unitOfWork.ProblemSubmission.GetAsync(command.Id, cancellationToken);
        if (problemSubmission == null) return Error.NotFound();

        problemSubmission.Code = command.Code ?? problemSubmission.Code;
        problemSubmission.Attempts = command.Attempts;
        problemSubmission.Score = command.Score;
        problemSubmission.IsFlagged = command.IsFlagged;
        problemSubmission.FlagReason = command.FlagReason ?? problemSubmission.FlagReason;
        problemSubmission.ProgLanguageId = command.ProgLanguageId;
        problemSubmission.UpdatedAt = DateTime.Now;


        var result = await _unitOfWork.CommitAsync(cancellationToken);

        /*return result > 0
            ? problemSubmission.ToDto()
            : Error.Failure();*/

        return Error.Failure();
    }
}

public class UpdateProblemSubmissionCommandValidator : AbstractValidator<UpdateProblemSubmitCommand>
{
    public UpdateProblemSubmissionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
        RuleFor(x => x.ProgLanguageId)
            .NotEmpty();

        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Attempts).NotEmpty();
        RuleFor(x => x.Score).NotEmpty();
    }
}