using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Submissions.ProblemSubmissions.Queries;

public record GetProblemSubmissionByIdQuery(Guid ProblemSubmissionId) : IRequest<ErrorOr<ProblemSubmitResponse>>;

public class GetProblemSubmissionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSubmissionByIdQuery, ErrorOr<ProblemSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemSubmitResponse>> Handle(GetProblemSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var submission = await _unitOfWork.ProblemSubmission.GetWithOutputsAsync(request.ProblemSubmissionId, cancellationToken);
        if (submission is null) return Error.NotFound();

        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(submission.QuestionId, cancellationToken);

        return submission.ToProblemSubmissionDto(testCases)!;
    }
}

public class GetProblemSubmissionByIdQueryValidator : AbstractValidator<GetProblemSubmissionByIdQuery>
{
    public GetProblemSubmissionByIdQueryValidator()
    {
        RuleFor(x => x.ProblemSubmissionId).NotEqual(Guid.Empty);
    }
}