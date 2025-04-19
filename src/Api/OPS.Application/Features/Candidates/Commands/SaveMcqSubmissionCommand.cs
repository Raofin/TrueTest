using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Candidates.Commands;

public record McqSubmissionRequest(Guid QuestionId, string CandidateAnswerOptions);

public record SaveMcqSubmissionsCommand(Guid ExamId, List<McqSubmissionRequest> Submissions)
    : IRequest<ErrorOr<Success>>;

public class SaveMcqSubmissionsCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveMcqSubmissionsCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<Success>> Handle(SaveMcqSubmissionsCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var isValidCandidate = await _unitOfWork.ExamCandidate
            .IsValidCandidate(userAccountId, request.ExamId, cancellationToken);

        if (!isValidCandidate) return Error.Unauthorized();

        foreach (var req in request.Submissions)
        {
            var submission = await _unitOfWork.McqSubmission
                .GetByAccountIdAsync(req.QuestionId, userAccountId, cancellationToken);

            if (submission is not null)
            {
                submission.AnswerOptions = req.CandidateAnswerOptions;
            }
            else
            {
                var question = await _unitOfWork.Question
                    .GetWithMcqOption(req.QuestionId, cancellationToken);

                if (question is null) return Error.Unexpected(description: $"Invalid question: {req.QuestionId}");

                submission = new McqSubmission
                {
                    AnswerOptions = req.CandidateAnswerOptions,
                    AccountId = userAccountId,
                    McqOptionId = question.McqOption!.Id,
                    QuestionId = req.QuestionId
                };

                _unitOfWork.McqSubmission.Add(submission);
            }
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class SaveMcqSubmissionsCommandValidator : AbstractValidator<SaveMcqSubmissionsCommand>
{
    public SaveMcqSubmissionsCommandValidator()
    {
        RuleForEach(x => x.Submissions).ChildRules(sub =>
        {
            sub.RuleFor(x => x.QuestionId)
                .IsValidGuid();

            sub.RuleFor(x => x.CandidateAnswerOptions)
                .NotEmpty()
                .Matches("^([1-4](,[1-4]){0,3})?$")
                .WithMessage("AnswerOptions must contain numbers 1-4, separated by commas.");
        });
    }
}