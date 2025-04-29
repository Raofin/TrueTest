using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.Written.Command;

public record UpdateWrittenCommand(
    Guid QuestionId,
    string? StatementMarkdown,
    decimal? Points,
    bool? HasLongAnswer,
    DifficultyType? DifficultyType) : IRequest<ErrorOr<WrittenQuestionResponse>>;

public class UpdateWrittenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWrittenCommand, ErrorOr<WrittenQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenQuestionResponse>> Handle(UpdateWrittenCommand request,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWithExamAsync(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        if (question.Examination.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;
        question.HasLongAnswer = request.HasLongAnswer ?? question.HasLongAnswer;

        if (request.Points is not null)
        {
            question.Examination.WrittenPoints -= question.Points;
            question.Examination.WrittenPoints += request.Points.Value;
            question.Points = request.Points.Value;
        }

        question.DifficultyId = request.DifficultyType.HasValue
            ? (int)request.DifficultyType.Value
            : question.DifficultyId;

        await _unitOfWork.CommitAsync(cancellationToken);

        return question.MapToWrittenQuestionDto();
    }
}

public class UpdateWrittenCommandValidator : AbstractValidator<UpdateWrittenCommand>
{
    public UpdateWrittenCommandValidator()
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
    }
}