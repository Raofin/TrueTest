using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Features.ProblemSubmissions.Commands;

public record SaveProblemSubmissionCommand(
    Guid QuestionId,
    string Code,
    ProgLanguageType ProgLanguageType) : IRequest<ErrorOr<ProblemSubmitResponse>>;

public class SaveProblemSubmissionCommandHandler(
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider)
    : IRequestHandler<SaveProblemSubmissionCommand, ErrorOr<ProblemSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<ProblemSubmitResponse>> Handle(
        SaveProblemSubmissionCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userInfoProvider.AccountId().ThrowIfNull(typeof(UnauthorizedAccessException));
        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(request.QuestionId, cancellationToken);

        // TODO: Add compiler service to compile the code
        
        var submission = new ProblemSubmission
        {
            Code = request.Code,
            Attempts = 1,
            Score = 0,
            ProgLanguageId = (int)request.ProgLanguageType,
            AccountId = accountId,
            QuestionId = request.QuestionId,
            TestCaseOutputs = testCases.Select(
                testCase => new TestCaseOutput
                {
                    TestCaseId = testCase.Id,
                    ReceivedOutput = "Compiler error",
                    IsAccepted = false
                }).ToList()
        };

        return await SaveProblemSubmission(submission, testCases, cancellationToken);
    }

    private async Task<ErrorOr<ProblemSubmitResponse>> SaveProblemSubmission(
        ProblemSubmission submission, List<TestCase> testCases, CancellationToken cancellationToken)
    {
        var existingSubmission = await _unitOfWork.ProblemSubmission.GetWithOutputsAsync(
            submission.QuestionId, submission.AccountId, cancellationToken);

        if (existingSubmission is null)
        {
            _unitOfWork.ProblemSubmission.Add(submission);
        }
        else
        {
            existingSubmission.Code = submission.Code;
            existingSubmission.Attempts++;
            existingSubmission.Score = submission.Score;
            existingSubmission.ProgLanguageId = submission.ProgLanguageId;
            existingSubmission.TestCaseOutputs = existingSubmission.TestCaseOutputs
                .Zip(submission.TestCaseOutputs, (existing, updated) =>
                {
                    existing.ReceivedOutput = updated.ReceivedOutput;
                    existing.IsAccepted = updated.IsAccepted;
                    return existing;
                }).ToList();

            submission = existingSubmission;
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? submission.ToProblemSubmissionDto(testCases)!
            : Error.Failure();
    }
}

public class SaveProblemSubmissionCommandValidator : AbstractValidator<SaveProblemSubmissionCommand>
{
    public SaveProblemSubmissionCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEqual(Guid.Empty);
        RuleFor(x => x.ProgLanguageType).IsInEnum();
        RuleFor(x => x.Code).NotEmpty();
    }
}