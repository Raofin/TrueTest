using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record CreateProblemSolvingCommand(
    string StatementMarkdown,
    decimal Points,
    Guid ExaminationId,
    DifficultyType DifficultyType,
    List<TestCaseRequest> TestCases) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class CreateProblemSolvingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProblemSolvingCommand, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(CreateProblemSolvingCommand request,
        CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExaminationId, cancellationToken);
        if (examExists == null) return Error.NotFound();

        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Points = request.Points,
            ExaminationId = request.ExaminationId,
            DifficultyId = (int)request.DifficultyType,
            QuestionTypeId = (int)QuestionType.ProblemSolving
        };

        foreach (var tc in request.TestCases)
        {
            question.TestCases.Add(
                new TestCase
                {
                    Input = tc.Input,
                    Output = tc.Output,
                }
            );
        }

        _unitOfWork.Question.Add(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToProblemQuestionDto()
            : Error.Failure();
    }
}

public class CreateProblemSolvingCommandValidator : AbstractValidator<CreateProblemSolvingCommand>
{
    public CreateProblemSolvingCommandValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .MinimumLength(10);
        
        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
        
        RuleFor(x => x.ExaminationId)
            .NotEqual(Guid.Empty);
        
        RuleFor(x => x.DifficultyType)
            .IsInEnum();
        
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
