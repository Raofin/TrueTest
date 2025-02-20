using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.WrittenSubmissions.Commands;

public record CreateWrittenSubmissionCommand(
    Guid QuestionId,
    Guid AccountId,
    string Answer,
    decimal Score) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class CreateWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWrittenSubmissionCommand, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(CreateWrittenSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var questionExists = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
        if (questionExists == null) return Error.NotFound("Question not found.");

        var accountExists = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (accountExists == null) return Error.NotFound("Account not found.");

        var writtenSubmission = new WrittenSubmission
        {
            QuestionId = request.QuestionId,
            AccountId = request.AccountId,
            Answer = request.Answer,
            Score = request.Score
        };

        _unitOfWork.WrittenSubmission.Add(writtenSubmission);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? writtenSubmission.ToDto()
            : Error.Failure("The WrittenSubmission could not be saved.");
    }
}

public class CreateWrittenSubmissionCommandValidator : AbstractValidator<CreateWrittenSubmissionCommand>
{
    public CreateWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.Answer).NotEmpty();

        RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
    }
}