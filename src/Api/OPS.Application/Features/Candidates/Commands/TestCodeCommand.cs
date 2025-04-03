using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record TestCodeResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string ReceivedOutput,
    TimeSpan ExecutionTime
);

public record TestCodeCommand(Guid QuestionId, string Code, ProgLanguageType ProgLanguageType)
    : IRequest<ErrorOr<List<TestCodeResponse>>>;

public class TestCodeCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<TestCodeCommand, ErrorOr<List<TestCodeResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<TestCodeResponse>>> Handle(TestCodeCommand request,
        CancellationToken cancellationToken)
    {
        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(request.QuestionId, cancellationToken);

        // TODO: Add compiler service to compile the code

        return testCases.Select(
            testCase => new TestCodeResponse(
                testCase.Id,
                false,
                "Compiler error",
                new TimeSpan(10)
            )
        ).ToList();
    }
}

public class TestCodeCommandValidator : AbstractValidator<TestCodeCommand>
{
    public TestCodeCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.ProgLanguageType)
            .IsInEnum();
    }
}