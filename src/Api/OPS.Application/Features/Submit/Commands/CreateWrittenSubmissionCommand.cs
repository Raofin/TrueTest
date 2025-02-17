using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OPS.Application.Contracts.Submit;
using OPS.Application.Extensions;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Submit.Commands;

public record CreateWrittenSubmissionCommand(
    Guid QuestionId,
    Guid AccountId,
    string Answer,
    decimal Score
) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class CreateWrittenSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWrittenSubmissionCommand, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(CreateWrittenSubmissionCommand request,
        CancellationToken cancellationToken)
    {

        var questionExists = await _unitOfWork.Question.GetAsync(request.QuestionId,cancellationToken);
        if (questionExists == null)
        {
            return Error.NotFound("Question not found."); 
        }

        var accountExists = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (accountExists == null)
        {
            return Error.NotFound("Account not found."); 
        }

        var WrittenSubmission = new WrittenSubmission
        {
            QuestionId = request.QuestionId, 
            AccountId = request.AccountId,   
            Answer = request.Answer,         
            Score = request.Score,         
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _unitOfWork.WrittenSubmission.Add(WrittenSubmission);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? WrittenSubmission.ToDto()
            : Error.Failure("The WrittenSubmission could not be saved.");
    }
}

public class CreateWrittenSubmissionCommandValidator : AbstractValidator<CreateWrittenSubmissionCommand>
{
    public CreateWrittenSubmissionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
        .NotEmpty().WithMessage("QuestionId is required.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId is required.");

        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("Answer is required.")
            .MaximumLength(5000).WithMessage("Answer cannot exceed 5000 characters."); 

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100."); 
    }
}