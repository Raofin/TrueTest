﻿using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record TestCodeResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string? ReceivedOutput,
    string? ErrorMessage,
    string? Exception,
    int? ExecutionTime
);

public record SaveProblemSubmissionsCommand(Guid ExamId, Guid QuestionId, string Code, LanguageId Language)
    : IRequest<ErrorOr<List<TestCodeResponse>>>;

public class SaveProblemSubmissionsCommandHandler(
    IUnitOfWork unitOfWork,
    IUserProvider userProvider,
    IOneCompilerService oneCompilerService)
    : IRequestHandler<SaveProblemSubmissionsCommand, ErrorOr<List<TestCodeResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;
    private readonly IOneCompilerService _oneCompiler = oneCompilerService;

    public async Task<ErrorOr<List<TestCodeResponse>>> Handle(
        SaveProblemSubmissionsCommand request, CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();

        var isValidCandidate = await _unitOfWork.ExamCandidate
            .IsValidCandidate(userAccountId, request.ExamId, cancellationToken);

        if (!isValidCandidate) return Error.Forbidden();

        var submission = await _unitOfWork.ProblemSubmission
            .GetAsync(request.QuestionId, userAccountId, cancellationToken);

        var testResults = await RunTestCasesAsync(request, cancellationToken);
        var isAccepted = testResults.All(tc => tc.IsAccepted);

        var points = await _unitOfWork.Question.GetPointsAsync(request.QuestionId, cancellationToken);

        UpsertSubmission(submission, request, userAccountId, isAccepted, points);

        await _unitOfWork.CommitAsync(cancellationToken);

        return testResults;
    }

    private void UpsertSubmission(
        ProblemSubmission? existing,
        SaveProblemSubmissionsCommand request,
        Guid accountId,
        bool isAccepted,
        decimal points)
    {
        if (existing is not null)
        {
            existing.Code = request.Code;
            existing.Attempts++;
            existing.LanguageId = request.Language.ToString();
            existing.Score = isAccepted ? points : 0;
        }
        else
        {
            var newSubmission = new ProblemSubmission
            {
                Code = request.Code,
                LanguageId = request.Language.ToString(),
                Attempts = 1,
                Score = isAccepted ? points : 0,
                AccountId = accountId,
                QuestionId = request.QuestionId
            };

            _unitOfWork.ProblemSubmission.Add(newSubmission);
        }
    }

    private async Task<List<TestCodeResponse>> RunTestCasesAsync(
        SaveProblemSubmissionsCommand request, CancellationToken ct)
    {
        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(request.QuestionId, ct);
        var responses = new List<TestCodeResponse>();

        foreach (var testCase in testCases)
        {
            var response = await ExecuteTestCaseAsync(request, testCase);
            responses.Add(response);
        }

        return responses;
    }

    private async Task<TestCodeResponse> ExecuteTestCaseAsync(
        SaveProblemSubmissionsCommand request, TestCase testCase)
    {
        var result = await _oneCompiler.CodeRunAsync(request.Language, request.Code, testCase.Input);

        return new TestCodeResponse(
            testCase.Id,
            IsAccepted(result, testCase.ExpectedOutput),
            result.Stdout,
            result.Stderr,
            result.Exception,
            result.ExecutionTime
        );
    }

    private static bool IsAccepted(CodeRunResponse result, string expectedOutput)
    {
        return result.Stderr == null
               && result.Exception == null
               && expectedOutput.TrimEnd() == result.Stdout?.TrimEnd();
    }
}

public class SaveProblemSubmissionsCommandValidator : AbstractValidator<SaveProblemSubmissionsCommand>
{
    public SaveProblemSubmissionsCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .IsValidGuid();

        RuleFor(x => x.QuestionId)
            .IsValidGuid();

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.Language)
            .IsInEnum();
    }
}