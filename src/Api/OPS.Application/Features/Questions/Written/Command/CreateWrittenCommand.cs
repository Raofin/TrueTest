using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.Written.Command;

public record CreateWrittenCommand(
    Guid ExamId,
    string StatementMarkdown,
    decimal Points,
    bool HasLongAnswer,
    DifficultyType DifficultyType) : IRequest<ErrorOr<WrittenQuestionResponse>>;

public class CreateWrittenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWrittenCommand, ErrorOr<WrittenQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenQuestionResponse>> Handle(
        CreateWrittenCommand request, CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (examExists == null) return Error.NotFound();

        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Points = request.Points,
            ExaminationId = request.ExamId,
            DifficultyId = (int)request.DifficultyType,
            QuestionTypeId = (int)QuestionType.Written,
            HasLongAnswer = request.HasLongAnswer
        };

        _unitOfWork.Question.Add(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.MapToWrittenQuestionDto()
            : Error.Failure();
    }
}

public class CreateWrittenCommandValidator : AbstractValidator<CreateWrittenCommand>
{
    public CreateWrittenCommandValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Points)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.DifficultyType)
            .NotEmpty()
            .IsInEnum();
    }
}