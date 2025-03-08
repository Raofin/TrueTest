using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Features.Questions.Mcq.Commands;

public record UpdateMcqCommand(
    Guid Id,
    string? StatementMarkdown,
    decimal? Points,
    DifficultyType? DifficultyType,
    UpdateMcqOptionRequest? McqOption) : IRequest<ErrorOr<McqQuestionResponse>>;

public class UpdateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMcqCommand, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(
        UpdateMcqCommand command, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithMcqOption(command.Id, cancellationToken);
        if (question is null) return Error.NotFound();
        question.McqOption.ThrowIfNull();

        question.StatementMarkdown = command.StatementMarkdown ?? question.StatementMarkdown;
        question.Points = command.Points ?? question.Points;
        question.DifficultyId = command.DifficultyType.HasValue ? (int)command.DifficultyType.Value : question.DifficultyId;

        if(command.McqOption is not null)
        {
            question.McqOption.Option1 = command.McqOption.Option1 ?? question.McqOption.Option1;
            question.McqOption.Option2 = command.McqOption.Option2 ?? question.McqOption.Option2;
            question.McqOption.Option3 = command.McqOption.Option3 ?? question.McqOption.Option3;
            question.McqOption.Option4 = command.McqOption.Option4 ?? question.McqOption.Option4;
            question.McqOption.IsMultiSelect = command.McqOption.IsMultiSelect ?? question.McqOption.IsMultiSelect;
            question.McqOption.AnswerOptions = command.McqOption.AnswerOptions ?? question.McqOption.AnswerOptions;
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToMcqQuestionDto()
            : Error.Failure();
    }
}

public class UpdateMcqCommandValidator : AbstractValidator<UpdateMcqCommand>
{
    public UpdateMcqCommandValidator()
    {
        RuleFor(x => x.Id)
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

        RuleFor(x => x.IsMultiSelect)
            .NotNull()
            .When(x => x.IsMultiSelect.HasValue);
    }
}