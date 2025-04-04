using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record TestCaseRequest(string Input, string Output);

public record CreateProblemQuestionRequest(
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    List<TestCaseRequest> TestCases);

public record CreateProblemSolvingCommand(Guid ExamId, List<CreateProblemQuestionRequest> ProblemQuestions)
    : IRequest<ErrorOr<List<ProblemQuestionResponse>>>;

public class CreateProblemSolvingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProblemSolvingCommand, ErrorOr<List<ProblemQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuestionResponse>>> Handle(CreateProblemSolvingCommand request,
        CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (exam == null) return Error.NotFound();

        if (exam.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        var questions = new List<Question>();

        foreach (var problem in request.ProblemQuestions)
        {
            var question = new Question
            {
                StatementMarkdown = problem.StatementMarkdown,
                Points = problem.Points,
                ExaminationId = request.ExamId,
                DifficultyId = (int)problem.DifficultyType,
                QuestionTypeId = (int)QuestionType.ProblemSolving
            };

            foreach (var tc in problem.TestCases)
            {
                question.TestCases.Add(new TestCase
                {
                    Input = tc.Input,
                    ExpectedOutput = tc.Output
                });
            }

            questions.Add(question);
        }

        exam.ProblemSolvingPoints += questions.Sum(q => q.Points);

        _unitOfWork.Question.AddRange(questions);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? questions.Select(q => q.MapToProblemQuestionDto()).ToList()
            : Error.Unexpected();
    }
}

public class CreateProblemSolvingCommandValidator : AbstractValidator<CreateProblemSolvingCommand>
{
    public CreateProblemSolvingCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .IsValidGuid();

        RuleFor(x => x.ProblemQuestions)
            .NotEmpty();

        RuleForEach(x => x.ProblemQuestions)
            .SetValidator(new ProblemQuestionRequestValidator());
    }
}

public class ProblemQuestionRequestValidator : AbstractValidator<CreateProblemQuestionRequest>
{
    public ProblemQuestionRequestValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .MinimumLength(10);

        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.DifficultyType)
            .IsInEnum();

        RuleFor(x => x.TestCases)
            .NotEmpty()
            .WithMessage("At least one test case is required for a problem-solving question.");

        RuleForEach(x => x.TestCases)
            .SetValidator(new TestCaseRequestValidator());
    }
}

public class TestCaseRequestValidator : AbstractValidator<TestCaseRequest>
{
    public TestCaseRequestValidator()
    {
        RuleFor(x => x.Input).NotEmpty();
        RuleFor(x => x.Output).NotEmpty();
    }
}