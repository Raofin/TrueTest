using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record ExamResultResponse(
    ExamResponse Exam,
    AccountBasicInfoResponse Account,
    ResultResponse? Result,
    SubmissionResponse Submission
);

public record GetResultByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamResultResponse>>;

public class GetResultsByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetResultByCandidateQuery, ErrorOr<ExamResultResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResultResponse>> Handle(GetResultByCandidateQuery request,
        CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, request.AccountId, cancellationToken);

        if (exam is null) return Error.NotFound("Exam not found.");

        return new ExamResultResponse(
            exam.MapToDto(),
            exam.ExamCandidates.First().Account!.MapToBasicInfoDto(),
            exam.ExamCandidates.First().MapToResultDto(),
            new SubmissionResponse(
                exam.Questions
                    .Where(q => q.ProblemSubmissions.Count != 0)
                    .Select(ToProblemSubmissionDto).ToList(),
                exam.Questions
                    .Where(q => q.WrittenSubmissions.Count != 0)
                    .Select(ToWrittenSubmissionDto).ToList(),
                exam.Questions
                    .Where(q => q.McqSubmissions.Count != 0)
                    .Select(ToMcqSubmissionDto).ToList()
            )
        );
    }

    private static ProblemSubmissionResponse? ToProblemSubmissionDto(Question question)
    {
        if (question.ProblemSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.ProblemSubmissions.First();

        return new ProblemSubmissionResponse(
            submission.QuestionId,
            submission.Id,
            submission.Code,
            submission.Attempts,
            submission.Score,
            submission.IsFlagged,
            submission.FlagReason,
            (ProgLanguageType)submission.ProgLanguageId,
            submission.TestCaseOutputs.Select(
                to => new TestCaseOutputResponse(to.TestCaseId, to.IsAccepted, to.ReceivedOutput)
            ).ToList()
        );
    }

    private static WrittenSubmissionResponse? ToWrittenSubmissionDto(Question question)
    {
        if (question.WrittenSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.WrittenSubmissions.First();

        return new WrittenSubmissionResponse(
            question.Id,
            submission.Id,
            submission.Answer,
            submission.Score
        );
    }

    private static McqSubmissionResponse? ToMcqSubmissionDto(Question question)
    {
        if (question.McqSubmissions.FirstOrDefault() is null)
            return null;

        var submission = question.McqSubmissions.First();

        return new McqSubmissionResponse(
            question.Id,
            submission.Id,
            submission.AnswerOptions,
            submission.Score
        );
    }
}

public class GetResultByCandidateQueryValidator : AbstractValidator<GetResultByCandidateQuery>
{
    public GetResultByCandidateQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}