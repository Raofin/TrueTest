﻿using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Candidates.Commands;

public record WrittenSubmissionRequest(Guid QuestionId, string CandidateAnswer);

public record SaveWrittenSubmissionsCommand(Guid ExamId, List<WrittenSubmissionRequest> Submissions)
    : IRequest<ErrorOr<Success>>;

public class SaveWrittenSubmissionsCommandHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
    : IRequestHandler<SaveWrittenSubmissionsCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;

    public async Task<ErrorOr<Success>> Handle(SaveWrittenSubmissionsCommand request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();

        var isValidCandidate = await _unitOfWork.ExamCandidate
            .IsValidCandidate(userAccountId, request.ExamId, cancellationToken);

        if (!isValidCandidate) return Error.Forbidden();

        foreach (var req in request.Submissions)
        {
            var submission = await _unitOfWork.WrittenSubmission
                .GetByAccountIdAsync(req.QuestionId, userAccountId, cancellationToken);

            if (submission is not null)
            {
                submission.Answer = req.CandidateAnswer;
            }
            else
            {
                submission = new WrittenSubmission
                {
                    Answer = req.CandidateAnswer,
                    QuestionId = req.QuestionId,
                    AccountId = userAccountId
                };

                _unitOfWork.WrittenSubmission.Add(submission);
            }
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success;
    }
}

public class SaveWrittenSubmissionsCommandValidator : AbstractValidator<SaveWrittenSubmissionsCommand>
{
    public SaveWrittenSubmissionsCommandValidator()
    {
        RuleForEach(x => x.Submissions).ChildRules(sub =>
        {
            sub.RuleFor(x => x.QuestionId)
                .IsValidGuid();

            sub.RuleFor(x => x.CandidateAnswer)
                .NotEmpty();
        });
    }
}