using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Features.Questions.Mcq.Commands;

public record UpdateMcqOptionRequest(
    string? Option1,
    string? Option2,
    string? Option3,
    string? Option4,
    bool? IsMultiSelect,
    string? AnswerOptions
);

public record UpdateMcqCommand(
    Guid QuestionId,
    string? StatementMarkdown,
    decimal? Points,
    DifficultyType? DifficultyType,
    UpdateMcqOptionRequest? McqOption) : IRequest<ErrorOr<McqQuestionResponse>>;

public class UpdateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMcqCommand, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(
        UpdateMcqCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithMcqOption(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        if (question.Examination.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");
        
        question.McqOption.ThrowIfNull();
        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;

        if (request.Points is not null)
        {
            question.Examination.McqPoints -= question.Points;
            question.Examination.TotalPoints -= question.Points;

            question.Points = request.Points.Value;
            question.Examination.McqPoints += question.Points;
            question.Examination.TotalPoints += question.Points;
        }

        question.DifficultyId = request.DifficultyType.HasValue
            ? (int)request.DifficultyType.Value
            : question.DifficultyId;

        if (request.McqOption is not null)
        {
            question.McqOption.Option1 = request.McqOption.Option1 ?? question.McqOption.Option1;
            question.McqOption.Option2 = request.McqOption.Option2 ?? question.McqOption.Option2;
            question.McqOption.Option3 = request.McqOption.Option3 ?? question.McqOption.Option3;
            question.McqOption.Option4 = request.McqOption.Option4 ?? question.McqOption.Option4;
            question.McqOption.IsMultiSelect = request.McqOption.IsMultiSelect ?? question.McqOption.IsMultiSelect;
            question.McqOption.AnswerOptions = request.McqOption.AnswerOptions ?? question.McqOption.AnswerOptions;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return question.MapToMcqQuestionDto();
    }
}

public class UpdateMcqCommandValidator : AbstractValidator<UpdateMcqCommand>
{
    public UpdateMcqCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();

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

        RuleFor(x => x.McqOption)
            .NotNull()
            .When(x => x.McqOption is not null)
            .SetValidator(new UpdateMcqOptionRequestValidator()!);
    }
}

public class UpdateMcqOptionRequestValidator : AbstractValidator<UpdateMcqOptionRequest>
{
    public UpdateMcqOptionRequestValidator()
    {
        RuleFor(x => x.Option1)
            .NotEmpty()
            .When(x => x.Option1 != null);

        RuleFor(x => x.Option2)
            .NotEmpty()
            .When(x => x.Option2 != null);

        RuleFor(x => x.Option3)
            .NotEmpty()
            .When(x => x.Option3 != null);

        RuleFor(x => x.Option4)
            .NotEmpty()
            .When(x => x.Option4 != null);

        RuleFor(x => x.AnswerOptions)
            .Matches("^([1-4](,[1-4]){0,3})?$")
            .WithMessage("AnswerOptions must contain numbers 1-4, separated by commas.")
            .When(x => !string.IsNullOrEmpty(x.AnswerOptions));
    }
}