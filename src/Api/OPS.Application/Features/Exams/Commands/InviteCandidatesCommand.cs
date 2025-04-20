using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Features.Exams.Commands;

public record InviteCandidatesCommand(Guid ExamId, List<string> Emails) : IRequest<ErrorOr<Success>>;

public class InviteCandidatesCommandHandler(IUnitOfWork unitOfWork, IAccountEmails accountEmails)
    : IRequestHandler<InviteCandidatesCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccountEmails _accountEmails = accountEmails;

    public async Task<ErrorOr<Success>> Handle(InviteCandidatesCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (exam is null) return Error.NotFound();

        var emailsToInvite = request.Emails.Distinct().ToList();

        var accounts = await _unitOfWork.Account.GetByEmailsAsync(emailsToInvite, cancellationToken);

        var existingCandidates = await _unitOfWork.ExamCandidate.GetEmailsByExamAsync(
            request.ExamId, emailsToInvite, cancellationToken
        );

        var candidates = new List<ExamCandidate>();

        foreach (var email in emailsToInvite)
        {
            if (existingCandidates.Contains(email)) continue;

            var examCandidate = new ExamCandidate
            {
                CandidateEmail = email,
                ExaminationId = request.ExamId
            };

            var account = accounts.FirstOrDefault(a => a.Email == email);
            if (account != null) examCandidate.AccountId = account.Id;

            candidates.Add(examCandidate);
        }

        if (candidates.Count != 0)
        {
            _unitOfWork.ExamCandidate.AddRange(candidates);
            await _unitOfWork.CommitAsync(cancellationToken);

            _accountEmails.SendExamInvitation(candidates.Select(c => c.CandidateEmail).ToList(),
                exam.Title, exam.OpensAt, exam.DurationMinutes, cancellationToken);
        }

        return Result.Success;
    }
}

public class InviteCandidatesCommandValidator : AbstractValidator<InviteCandidatesCommand>
{
    public InviteCandidatesCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .IsValidGuid();

        RuleFor(x => x.Emails)
            .NotEmpty()
            .Must(emails => emails.All(email => !string.IsNullOrWhiteSpace(email)));
    }
}