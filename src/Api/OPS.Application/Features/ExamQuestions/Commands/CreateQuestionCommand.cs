using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.ExamQuestions.Commands;

public record CreateQuestionCommand(
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyId,
    QuestionType QuestionTypeId,
    bool IsActive) : IRequest<ErrorOr<QuestionResponse>>;

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
            DifficultyId = (int)request.DifficultyId,
            QuestionTypeId = (int)request.QuestionTypeId
        };

        _unitOfWork.Question.Add(question);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.ToDto()
            : Error.Failure();
    }
}

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.StatementMarkdown)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Score)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ExaminationId)
            .NotEmpty();

        RuleFor(x => x.DifficultyId)
            .NotEmpty()
            .IsInEnum();

        RuleFor(x => x.QuestionTypeId)
            .NotEmpty()
            .IsInEnum();

        RuleFor(x => x.IsActive)
            .NotNull();
    }
}