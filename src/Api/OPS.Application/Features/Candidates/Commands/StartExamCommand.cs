using System.Diagnostics.CodeAnalysis;
using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record StartExamCommand(Guid ExamId) : IRequest<ErrorOr<ExamStartResponse>>;

public class StartExamCommandHandler(IUnitOfWork unitOfWork, IUserInfoProvider userInfoProvider)
    : IRequestHandler<StartExamCommand, ErrorOr<ExamStartResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<ExamStartResponse>> Handle(
        StartExamCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var candidate = await _unitOfWork.Exam.GetCandidateAsync(
            request.ExamId, userAccountId, cancellationToken);

        if (candidate == null)
            return Error.Forbidden(description: "Candidate was not invited to this exam");

        if (candidate.SubmittedAt < DateTime.UtcNow)
            return Error.Forbidden(description: "Exam is already submitted or ended");

        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, userAccountId, cancellationToken);

        if (exam is null)
            return Error.Unexpected(description: "Invalid exam");

        if (candidate.StartedAt == null)
        {
            var now = DateTime.UtcNow;
            candidate.StartedAt = now;
            candidate.SubmittedAt = now.AddMinutes(exam.DurationMinutes);

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        return new ExamStartResponse(
            exam.Id,
            candidate.StartedAt.Value,
            candidate.StartedAt.Value.AddMinutes(exam.DurationMinutes),
            exam.MapToQuestionDto(),
            new SubmitResponse(
                exam.Questions
                    .Where(q => q.ProblemSubmissions.Count != 0)
                    .Select(ToProblemSubmitDto).ToList(),
                exam.Questions
                    .Where(q => q.WrittenSubmissions.Count != 0)
                    .Select(ToWrittenSubmitDto).ToList(),
                exam.Questions
                    .Where(q => q.McqSubmissions.Count != 0)
                    .Select(ToMcqSubmitDto).ToList()
            )
        );
    }

    [ExcludeFromCodeCoverage]
    private static ProblemSubmitResponse? ToProblemSubmitDto(Question question)
    {
        if (question.ProblemSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.ProblemSubmissions.First();

        return new ProblemSubmitResponse(
            question.Id,
            submission.Id,
            submission.Code,
            Enum.TryParse(submission.LanguageId, out LanguageId languageId)
                ? languageId
                : LanguageId.text
        );
    }

    [ExcludeFromCodeCoverage]
    private static WrittenSubmitResponse? ToWrittenSubmitDto(Question question)
    {
        if (question.WrittenSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.WrittenSubmissions.First();

        return new WrittenSubmitResponse(
            question.Id,
            submission.Id,
            submission.Answer
        );
    }

    [ExcludeFromCodeCoverage]
    private static McqSubmitResponse? ToMcqSubmitDto(Question question)
    {
        if (question.McqSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.McqSubmissions.First();

        return new McqSubmitResponse(
            question.Id,
            submission.Id,
            submission.AnswerOptions
        );
    }
}

public class StartExamCommandValidator : AbstractValidator<StartExamCommand>
{
    public StartExamCommandValidator()
    {
        RuleFor(x => x.ExamId).IsValidGuid();
    }
}