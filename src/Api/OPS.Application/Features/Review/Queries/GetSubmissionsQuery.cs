using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Domain.Contracts.Repository.Submissions;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record GetSubmissionsQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamSubmissionResponse>>;

public class GetSubmissionsQueryHandler(
    IProblemSubmissionRepository problemSubmissionRepo,
    IWrittenSubmissionRepository writtenSubmissionRepo,
    IMcqSubmissionRepository mcqSubmissionRepo) : IRequestHandler<GetSubmissionsQuery, ErrorOr<ExamSubmissionResponse>>
{
    private readonly IProblemSubmissionRepository _problemSubmissionRepo = problemSubmissionRepo;
    private readonly IWrittenSubmissionRepository _writtenSubmissionRepo = writtenSubmissionRepo;
    private readonly IMcqSubmissionRepository _mcqSubmissionRepo = mcqSubmissionRepo;

    public async Task<ErrorOr<ExamSubmissionResponse>> Handle(
        GetSubmissionsQuery request, CancellationToken cancellationToken)
    {
        var problemTask = _problemSubmissionRepo.GetAllAsync(request.ExamId, request.AccountId, cancellationToken);
        var writtenTask = _writtenSubmissionRepo.GetAllAsync(request.ExamId, request.AccountId, cancellationToken);
        var mcqTask = _mcqSubmissionRepo.GetAllAsync(request.ExamId, request.AccountId, cancellationToken);

        await Task.WhenAll(writtenTask, problemTask, mcqTask);

        return new ExamSubmissionResponse(
            request.ExamId,
            request.AccountId,
            new SubmissionResponse(
                problemTask.Result.Select(ToSubmissionDto).ToList(),
                writtenTask.Result.Select(
                    ws => new WrittenSubmissionResponse(ws.QuestionId, ws.Id, ws.Answer, ws.Score)
                ).ToList(),
                mcqTask.Result.Select(
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