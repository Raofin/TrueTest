using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;
using OPS.Domain.Contracts.Core.OneCompiler;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record TestCodeResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string? ReceivedOutput,
    int? ExecutionTime
);

public record TestCodeCommand(Guid QuestionId, string Code, LanguageId LanguageId)
    : IRequest<ErrorOr<List<TestCodeResponse>>>;

public class TestCodeCommandHandler(IUnitOfWork unitOfWork, IOneCompilerApiService oneCompilerApiService)
    : IRequestHandler<TestCodeCommand, ErrorOr<List<TestCodeResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOneCompilerApiService _oneCompilerApi = oneCompilerApiService;

    public async Task<ErrorOr<List<TestCodeResponse>>> Handle(
        TestCodeCommand request, CancellationToken cancellationToken)
    {
        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(request.QuestionId, cancellationToken);

        var responses = new List<TestCodeResponse>();

        foreach (var testCase in testCases)
        {
            var result = await _oneCompilerApi.CodeRunAsync(
                request.LanguageId,
                request.Code,
                testCase.Input
            );

            var response = new TestCodeResponse(
                testCase.Id,
                testCase.ExpectedOutput.TrimEnd() == result.Stdout?.TrimEnd(),
                result.Stdout,
                result.ExecutionTime
            );

            responses.Add(response);
        }

        return responses;
    }
}

public class TestCodeCommandValidator : AbstractValidator<TestCodeCommand>
{
    public TestCodeCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .IsValidGuid();

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.LanguageId)
            .IsInEnum();
    }
}