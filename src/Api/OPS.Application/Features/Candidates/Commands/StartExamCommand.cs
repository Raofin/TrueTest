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
            return Error.Unauthorized(description: "Candidate was not invited to this exam");

        if (candidate.SubmittedAt != null || candidate.Examination.ClosesAt < DateTime.UtcNow)
            return Error.Unauthorized(description: "Exam is already submitted or ended");

        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, userAccountId, cancellationToken);

        exam.ThrowIfNull();

        if (candidate.StartedAt == null)
        {
            var now = DateTime.UtcNow;
            candidate.StartedAt = now;
            candidate.SubmittedAt = now > exam.ClosesAt ? exam.ClosesAt : now.AddMinutes(exam.DurationMinutes);

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        var examReview = new ExamStartResponse(
            exam.Id,
            candidate.StartedAt.Value,
            candidate.StartedAt.Value.AddMinutes(exam.DurationMinutes),
            new QuestionResponses(
                exam.Questions
                    .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
                    .Select(q => q.MapToProblemQuestionDto())
                    .ToList(),
                exam.Questions
                    .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
                    .Select(q => q.MapToWrittenQuestionDto())
                    .ToList(),
                exam.Questions
                    .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
                    .Select(q => q.MapToMcqQuestionDto())
                    .ToList()
            ),
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

        return examReview;
    }

    private static ProblemSubmitResponse? ToProblemSubmitDto(Question question)
    {
        if (question.ProblemSubmissions.FirstOrDefault() is null)
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
        if (question.WrittenSubmissions.FirstOrDefault() is null)
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
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}