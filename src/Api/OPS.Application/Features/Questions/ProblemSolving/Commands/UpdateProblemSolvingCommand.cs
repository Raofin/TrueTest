using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record UpdateProblemSolvingCommand(
    Guid Id,
    string? StatementMarkdown,
    decimal? Points,
    DifficultyType? DifficultyType,
    List<TestCaseUpdateRequest> TestCases) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class UpdateProblemSolvingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProblemSolvingCommand, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(
        UpdateProblemSolvingCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithTestCases(request.Id, cancellationToken);
        if (question is null) return Error.NotFound();

        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;
        question.Points = request.Points ?? question.Points;
        question.DifficultyId = request.DifficultyType.HasValue
            ? (int)request.DifficultyType.Value
            : question.DifficultyId;

        foreach (var tc in request.TestCases)
        {
            if (tc.TestCaseId.HasValue && tc.TestCaseId != Guid.Empty)
            {
                var testCase = await _unitOfWork.TestCase.GetAsync(tc.TestCaseId.Value, cancellationToken);
                testCase.ThrowIfNull();

                testCase.Input = tc.Input ?? testCase.Input;
                testCase.ExpectedOutput = tc.Output ?? testCase.ExpectedOutput;
            }
            else
            {
                question.TestCases.Add(
                    new TestCase
                    {
                        Input = tc.Input!,
                        ExpectedOutput = tc.Output!,
                    }
                );
            }
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToProblemQuestionDto()
            : Error.Failure();
    }
}

public class UpdateProblemSolvingCommandValidator : AbstractValidator<UpdateProblemSolvingCommand>
{
    public UpdateProblemSolvingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.StatementMarkdown)
            .MinimumLength(10)
            .When(x => !string.IsNullOrEmpty(x.StatementMarkdown));

        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .When(x => x.Points.HasValue);

        RuleFor(x => x.DifficultyType)
            .IsInEnum()
            .When(x => x.DifficultyType.HasValue);

        RuleForEach(x => x.TestCases)
            .SetValidator(new TestCaseUpdateRequestValidator());
    }
}

public class TestCaseUpdateRequestValidator : AbstractValidator<TestCaseUpdateRequest>
{
    public TestCaseUpdateRequestValidator()
    {
        RuleFor(x => x.Input)
            .NotEmpty()
            .When(x => x.TestCaseId == null);

        RuleFor(x => x.Output)
            .NotEmpty()
            .When(x => x.TestCaseId == null);

        RuleFor(x => x.TestCaseId)
            .Must(id => id == null || id != Guid.Empty);
    }
}