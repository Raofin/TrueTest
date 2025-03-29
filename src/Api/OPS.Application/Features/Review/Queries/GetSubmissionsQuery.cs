using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain.Contracts.Repository.Submissions;

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
                problemTask.Result.Select(q => q.ToSubmissionDto()).ToList(),
                writtenTask.Result.Select(q => q.ToSubmissionDto()).ToList(),
                mcqTask.Result.Select(q => q.ToSubmissionDto()).ToList()
            )
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