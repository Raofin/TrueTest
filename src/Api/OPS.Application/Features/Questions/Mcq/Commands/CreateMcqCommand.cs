using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.Mcq.Commands;

public record CreateMcqCommand(
    Guid ExamId,
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    CreateMcqOptionRequest McqOption) : IRequest<ErrorOr<McqQuestionResponse>>;

public class CreateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMcqCommand, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(
        CreateMcqCommand request, CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (examExists == null) return Error.NotFound();

        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Points = request.Points,
            ExaminationId = request.ExamId,
            DifficultyId = (int)request.DifficultyType,
            QuestionTypeId = (int)QuestionType.MCQ,
            McqOption = new McqOption
            {
                Option1 = request.McqOption.Option1,
                Option2 = request.McqOption.Option2,
                Option3 = request.McqOption.Option3,
                Option4 = request.McqOption.Option4,
                IsMultiSelect = request.McqOption.IsMultiSelect,
                AnswerOptions = request.McqOption.AnswerOptions
            }
        };

        _unitOfWork.Question.Add(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToMcqQuestionDto()
            : Error.Failure();
    }
}

public class CreateMcqCommandValidator : AbstractValidator<CreateMcqCommand>
{
    public CreateMcqCommandValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.ExamId)
            .NotEmpty();

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

        RuleFor(x => x.Option3)
            .Must(option => string.IsNullOrEmpty(option) || option.Length > 0);

        RuleFor(x => x.Option4)
            .Must(option => string.IsNullOrEmpty(option) || option.Length > 0);

        RuleFor(x => x.AnswerOptions)
            .NotEmpty()
            .Matches("^([1-4](,[1-4]){0,3})?$")
            .WithMessage("AnswerOptions must contain numbers 1-4, separated by commas.");
    }
}