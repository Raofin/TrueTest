using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.ExamQuestions.Commands;

public record UpdateQuestionCommand(
    Guid QuestionId,
    string? StatementMarkdown,
    decimal? Points,
    DifficultyType? DifficultyType,
    bool? IsActive,
    bool? IsDeleted) : IRequest<ErrorOr<QuestionResponse>>;

public class UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(command.QuestionId, cancellationToken);

        if (question is null) return Error.NotFound();

        question.StatementMarkdown = command.StatementMarkdown ?? question.StatementMarkdown;
        question.Points = command.Points ?? question.Points;
        question.DifficultyId = command.DifficultyType.HasValue ? (int)command.DifficultyType.Value : question.DifficultyId;
        question.IsActive = command.IsActive ?? question.IsActive;
        question.IsDeleted = command.IsDeleted ?? question.IsDeleted;
        question.UpdatedAt = DateTime.UtcNow;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToDto()
            : Error.Failure();
    }
}

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.StatementMarkdown)
            .MaximumLength(5000)
            .When(x => x.StatementMarkdown is not null);

        RuleFor(x => x.Points)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Points.HasValue);

        RuleFor(x => x.DifficultyType)
            .IsInEnum()
            .When(x => x.DifficultyType.HasValue);
    }
}