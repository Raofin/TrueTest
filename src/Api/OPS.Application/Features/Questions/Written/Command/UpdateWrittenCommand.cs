using ErrorOr;
using FluentValidation;
using MediatR;
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
        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
        if (question is null) return Error.NotFound();

        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;
        question.Points = request.Points ?? question.Points;
        question.HasLongAnswer = request.HasLongAnswer ?? question.HasLongAnswer;
        question.DifficultyId = request.DifficultyType.HasValue
            ? (int)request.DifficultyType.Value
            : question.DifficultyId;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToWrittenQuestionDto()
            : Error.Failure();
    }
}

public class UpdateWrittenCommandValidator : AbstractValidator<UpdateWrittenCommand>
{
    public UpdateWrittenCommandValidator()
    {
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
    }
}