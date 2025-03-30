using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;
using Throw;

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
        {
            return Error.Unauthorized(description: "Candidate was not invited to this exam");
        }

        if (candidate.SubmittedAt != null || candidate.Examination.ClosesAt > DateTime.UtcNow)
        {
            return Error.Unauthorized(description: "Exam is already submitted or ended");
        }

        if (candidate.StartedAt == null)
        {
            candidate.StartedAt = DateTime.UtcNow;
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, userAccountId, cancellationToken);

        exam.ThrowIfNull();

        var examReview = new ExamStartResponse(
            exam.Id,
            candidate.StartedAt.Value,
            candidate.StartedAt.Value.AddMinutes(exam.DurationMinutes),
            new QuestionResponses(
                exam.Questions.Select(q => q.MapToProblemQuestionDto()).ToList(),
                exam.Questions.Select(q => q.MapToWrittenQuestionDto()).ToList(),
                exam.Questions.Select(q => q.MapToMcqQuestionDto()).ToList()
            ),
            new SubmitResponse(
                exam.Questions.Select(ToProblemSubmitDto).ToList(),
                exam.Questions.Select(ToWrittenSubmitDto).ToList(),
                exam.Questions.Select(ToMcqSubmitDto).ToList()
            )
        );

        return examReview;
    }

    private static ProblemSubmitResponse? ToProblemSubmitDto(Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.ProblemSolving
            || question.ProblemSubmissions.FirstOrDefault() is not null)
            return null;

        var submission = question.ProblemSubmissions.First();

        return new ProblemSubmitResponse(
            question.Id,
            submission.Id,
            submission.Code,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(tco => new TestCaseOutputResponse(
                tco.TestCaseId,
                tco.IsAccepted,
                tco.ReceivedOutput
            )).ToList()
        );
    }

    private static WrittenSubmitResponse? ToWrittenSubmitDto(Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.Written ||
            question.WrittenSubmissions.FirstOrDefault() is not null)
            return null;

        var submission = question.WrittenSubmissions.First();

        return new WrittenSubmitResponse(
            question.Id,
            submission.Id,
            submission.Answer
        );
    }

    private static McqSubmitResponse? ToMcqSubmitDto(Question question)
    {
        if (question.QuestionTypeId != (int)QuestionType.MCQ ||
            question.McqSubmissions.FirstOrDefault() is not null)
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
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}