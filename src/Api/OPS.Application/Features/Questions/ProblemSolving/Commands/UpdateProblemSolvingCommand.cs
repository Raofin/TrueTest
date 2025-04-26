using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record TestCaseUpdateRequest(Guid? TestCaseId, string? Input, string? Output);

public record UpdateProblemSolvingCommand(
    Guid QuestionId,
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
        var question = await _unitOfWork.Question.GetWithTestCases(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        if (question.Examination.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;

        if (request.Points is not null)
        {
            question.Examination.ProblemSolvingPoints -= question.Points;
            question.Examination.ProblemSolvingPoints += request.Points.Value;
            question.Points = request.Points.Value;
        }

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

        await _unitOfWork.CommitAsync(cancellationToken);

        return question.MapToProblemQuestionDto();
    }
}

public class UpdateProblemSolvingCommandValidator : AbstractValidator<UpdateProblemSolvingCommand>
{
    public UpdateProblemSolvingCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .IsValidGuid();

        RuleFor(x => x.StatementMarkdown)
            .MinimumLength(1)
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