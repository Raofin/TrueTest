using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.Mcq.Commands;

public record CreateMcqOptionRequest(
    string Option1,
    string Option2,
    string? Option3,
    string? Option4,
    bool IsMultiSelect,
    string AnswerOptions
);

public record CreateMcqQuestionRequest(
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    CreateMcqOptionRequest McqOption
);

public record CreateMcqCommand(Guid ExamId, List<CreateMcqQuestionRequest> McqQuestions)
    : IRequest<ErrorOr<List<McqQuestionResponse>>>;

public class CreateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMcqCommand, ErrorOr<List<McqQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqQuestionResponse>>> Handle(
        CreateMcqCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (exam == null) return Error.NotFound();

        if (exam.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        var questions = request.McqQuestions.Select(
            mcq => new Question
            {
                StatementMarkdown = mcq.StatementMarkdown,
                Points = mcq.Points,
                ExaminationId = request.ExamId,
                DifficultyId = (int)mcq.DifficultyType,
                QuestionTypeId = (int)QuestionType.MCQ,
                McqOption = new McqOption
                {
                    Option1 = mcq.McqOption.Option1,
                    Option2 = mcq.McqOption.Option2,
                    Option3 = mcq.McqOption.Option3,
                    Option4 = mcq.McqOption.Option4,
                    IsMultiSelect = mcq.McqOption.IsMultiSelect,
                    AnswerOptions = mcq.McqOption.AnswerOptions
                }
            }
        ).ToList();

        var newPoints = questions.Sum(q => q.Points);
        exam.McqPoints += newPoints;
        exam.TotalPoints += newPoints;

        _unitOfWork.Question.AddRange(questions);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? questions.Select(q => q.MapToMcqQuestionDto()).ToList()
            : Error.Unexpected();
    }
}

public class CreateMcqCommandValidator : AbstractValidator<CreateMcqCommand>
{
    public CreateMcqCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.McqQuestions)
            .NotEmpty();

        RuleForEach(x => x.McqQuestions)
            .SetValidator(new CreateMcqQuestionRequestValidator());
    }
}

public class CreateMcqQuestionRequestValidator : AbstractValidator<CreateMcqQuestionRequest>
{
    public CreateMcqQuestionRequestValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.DifficultyType)
            .IsInEnum();

        RuleFor(x => x.McqOption)
            .NotNull()
            .SetValidator(new CreateMcqOptionRequestValidator());
    }
}

public class CreateMcqOptionRequestValidator : AbstractValidator<CreateMcqOptionRequest>
{
    public CreateMcqOptionRequestValidator()
    {
        RuleFor(x => x.Option1)
            .NotEmpty();

        RuleFor(x => x.Option2)
            .NotEmpty();

        RuleFor(x => x.AnswerOptions)
            .NotEmpty()
            .Matches("^([1-4](,[1-4]){0,3})?$")
            .WithMessage("AnswerOptions must contain numbers 1-4, separated by commas.");
    }
}