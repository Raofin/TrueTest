using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Submissions.Queries;

public record GetProblemSubmissionQuery(Guid ProblemSubmissionId) : IRequest<ErrorOr<ProblemSubmitResponse>>;

public class GetProblemSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSubmissionQuery, ErrorOr<ProblemSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemSubmitResponse>> Handle(GetProblemSubmissionQuery request, CancellationToken cancellationToken)
    {
        var submission = await _unitOfWork.ProblemSubmission.GetWithOutputsAsync(request.ProblemSubmissionId, cancellationToken);
        if (submission is null) return Error.NotFound();

        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(submission.QuestionId, cancellationToken);

        return submission.ToProblemSubmissionDto(testCases)!;
    }
}

public class GetProblemSubmissionQueryValidator : AbstractValidator<GetProblemSubmissionQuery>
{
    public GetProblemSubmissionQueryValidator()
    {
        RuleFor(x => x.ProblemSubmissionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}