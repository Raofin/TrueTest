using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.Exam.Commands;

public record CreateQuestionCommand(
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    Guid DifficultyId,
    Guid QuestionTypeId,
    bool IsActive
) : IRequest<ErrorOr<QuestionResponse>>;

public class CreateQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateQuestionCommand, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Score = request.Score,
            ExaminationId = request.ExaminationId,
            DifficultyId = request.DifficultyId,
            QuestionTypeId = request.QuestionTypeId,
            CreatedAt = DateTime.UtcNow,
            IsActive = request.IsActive,
            IsDeleted = false
        };

        _unitOfWork.Question.Add(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToDto()
            : Error.Failure("The exam could not be saved.");
    }
}

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty().WithMessage("Statement Markdown is required.")
            .Length(10, 2000).WithMessage("Statement Markdown must be between 10 and 2000 characters."); // Adjust max length as needed

        RuleFor(x => x.Score)
            .NotEmpty().WithMessage("Score is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Score must be a non-negative number."); // Or set an appropriate max value

        RuleFor(x => x.ExaminationId)
            .NotEmpty().WithMessage("ExaminationId is required.");

        RuleFor(x => x.DifficultyId)
            .NotEmpty().WithMessage("DifficultyId is required.");

        RuleFor(x => x.QuestionTypeId)
            .NotEmpty().WithMessage("QuestionTypeId is required.");
            
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive is required.");
    }
}