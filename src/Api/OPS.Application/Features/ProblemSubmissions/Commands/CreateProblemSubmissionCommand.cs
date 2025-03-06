using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.ProblemSubmissions.Commands;

public record CreateProblemSubmissionCommand(
    string Code,
    int Attempts,
    decimal Score,
    bool IsFlagged,
    string? FlagReason,
    int ProgLanguageId,
    Guid AccountId,
    Guid QuestionId
    ) : IRequest<ErrorOr<ProblemSubmissionResponse>>;

public class CreateProblemSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProblemSubmissionCommand, ErrorOr<ProblemSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemSubmissionResponse>> Handle(CreateProblemSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var accountExists = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (accountExists == null) return Error.NotFound();

        var questionExists = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);     
        if(questionExists == null) return Error.NotFound();        

        var ProblemSubmission = new ProblemSubmission
        {
            Code = request.Code,
            Attempts = request.Attempts,
            Score = request.Score,
            IsFlagged = request.IsFlagged,
            FlagReason = request.FlagReason,
            ProgLanguageId = request.ProgLanguageId,
            AccountId = request.AccountId,
            QuestionId = request.QuestionId
        };

        _unitOfWork.ProblemSubmission.Add(ProblemSubmission);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? ProblemSubmission.ToDto()
            : Error.Failure();
    }
}

public class CreateProblemSubmissionCommandValidator : AbstractValidator<CreateProblemSubmissionCommand>
{
    public CreateProblemSubmissionCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
        RuleFor(x => x.ProgLanguageId)
            .NotEmpty();

        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Attempts).NotEmpty();
        RuleFor(x => x.Score).NotEmpty();
    }
}