using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Candidates.Commands;

public record McqSubmissionRequest(Guid QuestionId, string CandidateAnswerOptions);

public record SaveMcqSubmissionsCommand(Guid ExamId, List<McqSubmissionRequest> Submissions)
    : IRequest<ErrorOr<Success>>;

public class SaveMcqSubmissionsCommandHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
    : IRequestHandler<SaveMcqSubmissionsCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;

    public async Task<ErrorOr<Success>> Handle(SaveMcqSubmissionsCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();

        var isValidCandidate = await _unitOfWork.ExamCandidate
            .IsValidCandidate(userAccountId, request.ExamId, cancellationToken);

        if (!isValidCandidate) return Error.Forbidden();

        foreach (var req in request.Submissions)
        {
            var submission = await _unitOfWork.McqSubmission
                .GetByAccountIdAsync(req.QuestionId, userAccountId, cancellationToken);

            var question = await _unitOfWork.Question.GetWithMcqOption(req.QuestionId, cancellationToken);

            var score = question!.McqOption!.AnswerOptions == req.CandidateAnswerOptions
                ? question.Points
                : 0;

            if (submission is not null)
            {
                submission.AnswerOptions = req.CandidateAnswerOptions;
                submission.Score = score;
            }
            else
            {
                submission = new McqSubmission
                {
                    AnswerOptions = req.CandidateAnswerOptions,
                    Score = score,
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