using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record GetSubmissionsQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamSubmissionResponse>>;

public class GetSubmissionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSubmissionsQuery, ErrorOr<ExamSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamSubmissionResponse>> Handle(
        GetSubmissionsQuery request, CancellationToken cancellationToken)
    {
        var problemSubmissions = await _unitOfWork.ProblemSubmission.GetAllAsync(
            request.ExamId, request.AccountId, cancellationToken);

        var writtenSubmissions = await _unitOfWork.WrittenSubmission.GetAllAsync(
            request.ExamId, request.AccountId, cancellationToken);

        var mcqSubmissions = await _unitOfWork.McqSubmission.GetAllAsync(
            request.ExamId, request.AccountId, cancellationToken);

        return new ExamSubmissionResponse(
            request.ExamId,
            request.AccountId,
            new SubmissionResponse(
                problemSubmissions.Select(ToSubmissionDto).ToList(),
                writtenSubmissions.Select(
                    ws => new WrittenSubmissionResponse(ws.QuestionId, ws.Id, ws.Answer, ws.Score)
                ).ToList(),
                mcqSubmissions.Select(
                    ms => new McqSubmissionResponse(ms.QuestionId, ms.Id, ms.AnswerOptions, ms.Score)
                ).ToList()
            )
        );
    }

    private static ProblemSubmissionResponse ToSubmissionDto(ProblemSubmission submission)
    {
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
}

public class GetSubmissionsQueryValidator : AbstractValidator<GetSubmissionsQuery>
{
    public GetSubmissionsQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}