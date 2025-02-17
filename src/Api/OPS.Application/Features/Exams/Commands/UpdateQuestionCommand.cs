
using Azure.Core;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateQuestionCommand(
    Guid QuestionId,
    string? StatementMarkdown,
    decimal? Score,
    Guid ExaminationId,  
    Guid DifficultyId,
    Guid QuestionTypeId,
    bool IsActive,
    bool IsDeleted
) : IRequest<ErrorOr<QuestionResponse>>;

public class UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(command.QuestionId, cancellationToken);

        if (question is null) return Error.NotFound("Question was not found");

        question.StatementMarkdown = command.StatementMarkdown;
        question.Score = (decimal)command.Score;
        question.ExaminationId = command.ExaminationId;
        question.DifficultyId = command.DifficultyId;
        question.QuestionTypeId = command.QuestionTypeId;
        question.UpdatedAt = DateTime.UtcNow;
        question.IsActive = command.IsActive;
        question.IsDeleted = false;
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToDto()
            : Error.Failure("The Question could not be saved.");
    }
}

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
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
