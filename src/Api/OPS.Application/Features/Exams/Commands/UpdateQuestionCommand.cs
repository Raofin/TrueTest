using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateQuestionCommand(
    Guid QuestionId,
    string? StatementMarkdown,
    decimal? Score,
    int? DifficultyId,
    int? QuestionTypeId,
    bool? IsActive,
    bool? IsDeleted) : IRequest<ErrorOr<QuestionResponse>>;

public class UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(command.QuestionId, cancellationToken);

        if (question is null) return Error.NotFound("Question was not found");

        question.StatementMarkdown = command.StatementMarkdown ?? question.StatementMarkdown;
        question.Score = command.Score ?? question.Score;
        question.DifficultyId = command.DifficultyId ?? question.DifficultyId;
        question.QuestionTypeId = command.QuestionTypeId ?? question.QuestionTypeId;
        question.IsActive = command.IsActive ?? question.IsActive;
        question.IsDeleted = command.IsDeleted ?? question.IsDeleted;

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
        RuleFor(x => x.StatementMarkdown).NotEmpty();

        RuleFor(x => x.Score).NotEmpty();

        RuleFor(x => x.DifficultyId).NotEmpty();

        RuleFor(x => x.QuestionTypeId).NotEmpty();

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive is required.");
    }
}