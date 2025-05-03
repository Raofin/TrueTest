using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Questions.Written.Command;

public record CreateWrittenQuestionRequest(
    string StatementMarkdown,
    decimal Points,
    DifficultyType DifficultyType,
    bool HasLongAnswer
);

public record CreateWrittenCommand(Guid ExamId, List<CreateWrittenQuestionRequest> WrittenQuestions)
    : IRequest<ErrorOr<List<WrittenQuestionResponse>>>;

public class CreateWrittenCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWrittenCommand, ErrorOr<List<WrittenQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenQuestionResponse>>> Handle(
        CreateWrittenCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (exam == null) return Error.NotFound();

        if (exam.IsPublished)
            return Error.Conflict(description: "Exam of this question is already published");

        var newQuestions = request.WrittenQuestions.Select(
            written => new Question
            {
                StatementMarkdown = written.StatementMarkdown,
                Points = written.Points,
                ExaminationId = request.ExamId,
                DifficultyId = (int)written.DifficultyType,
                QuestionTypeId = (int)QuestionType.Written,
                HasLongAnswer = written.HasLongAnswer
            }
        ).ToList();

        exam.WrittenPoints += newQuestions.Sum(q => q.Points);

        _unitOfWork.Question.AddRange(newQuestions);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? newQuestions.Select(q => q.MapToWrittenQuestionDto()).ToList()
            : Error.Unexpected();
    }
}

public class CreateWrittenCommandValidator : AbstractValidator<CreateWrittenCommand>
{
    public CreateWrittenCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .IsValidGuid();

        RuleFor(x => x.WrittenQuestions)
            .NotEmpty();

        RuleForEach(x => x.WrittenQuestions)
            .SetValidator(new CreateWrittenQuestionRequestValidator());
    }
}

public class CreateWrittenQuestionRequestValidator : AbstractValidator<CreateWrittenQuestionRequest>
{
    public CreateWrittenQuestionRequestValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(x => x.Points)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.DifficultyType)
            .IsInEnum();

        RuleFor(x => x.HasLongAnswer)
            .NotNull();
    }
}