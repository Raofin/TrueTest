using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record ProblemSubmissionRequest(
    Guid QuestionId,
    string Code,
    LanguageId Language);

public record SaveProblemSubmissionsCommand(Guid ExamId, List<ProblemSubmissionRequest> Submissions)
    : IRequest<ErrorOr<Success>>;

public class SaveProblemSubmissionsCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveProblemSubmissionsCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<Success>> Handle(
        SaveProblemSubmissionsCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var isValidCandidate = await _unitOfWork.ExamCandidate
            .IsValidCandidate(userAccountId, request.ExamId, cancellationToken);

        if (!isValidCandidate) return Error.Unauthorized();

        foreach (var req in request.Submissions)
        {
            var submission = await _unitOfWork.ProblemSubmission
                .GetAsync(req.QuestionId, userAccountId, cancellationToken);

            if (submission is not null)
            {
                submission.Code = req.Code;
                submission.Attempts++;
                submission.LanguageId = req.Language.ToString();
            }
            else
            {
                submission = new ProblemSubmission
                {
                    Code = req.Code,
                    LanguageId = req.Language.ToString(),
                    Attempts = 1,
                    AccountId = userAccountId,
                    QuestionId = req.QuestionId
                };

                _unitOfWork.ProblemSubmission.Add(submission);
            }
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class SaveProblemSubmissionsCommandValidator : AbstractValidator<SaveProblemSubmissionsCommand>
{
    public SaveProblemSubmissionsCommandValidator()
    {
        RuleForEach(x => x.Submissions).ChildRules(sub =>
        {
            sub.RuleFor(x => x.QuestionId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            sub.RuleFor(x => x.Language)
                .IsInEnum();

            sub.RuleFor(x => x.Code)
                .NotEmpty();
        });
    }
}