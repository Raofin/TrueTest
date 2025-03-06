using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.McqQuestions.Commands;

public record UpdateMcqQuestionCommand(
    Guid Id,
    string StatementMarkdown,
    decimal Score,
    DifficultyType DifficultyId,
    List<UpdateMcqOptionResponse> McqOptions
    ) : IRequest<ErrorOr<McqQuestionResponse>>;

public class UpdateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMcqQuestionCommand, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(UpdateMcqQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var mcqQuestion = await _unitOfWork.Question.GetAsync(command.Id, cancellationToken);

        if (mcqQuestion is null) return Error.NotFound(); 

        mcqQuestion.StatementMarkdown = command.StatementMarkdown ?? mcqQuestion.StatementMarkdown;
        mcqQuestion.Score = command.Score;
        mcqQuestion.DifficultyId = (int)command.DifficultyId;

        List<McqOptionResponse> options = new List<McqOptionResponse>();

        foreach (var option in command.McqOptions)
        {
            if (option.Id == Guid.Empty)
            {
                var mcqOption = new McqOption
                {
                    Option1 = option.Option1,
                    Option2 = option.Option2,
                    Option3 = option.Option3,
                    Option4 = option.Option4,
                    IsMultiSelect = option.IsMultiSelect,
                    AnswerOptions = option.AnswerOptions,
                    QuestionId = mcqQuestion.Id
                };
                _unitOfWork.McqOption.Add(mcqOption);
                options.Add(mcqOption.ToDto());
            }
            else
            {
                var mcqOption = await _unitOfWork.McqOption.GetAsync(option.Id, cancellationToken);
                if (mcqOption is null || mcqOption.QuestionId != mcqQuestion.Id) return Error.NotFound();

                mcqOption.Option1 = option.Option1 ?? mcqOption.Option1;
                mcqOption.Option2 = option.Option2 ?? mcqOption.Option2;
                mcqOption.Option3 = option.Option3 ?? mcqOption.Option3;
                mcqOption.Option4 = option.Option4 ?? mcqOption.Option4;
                mcqOption.IsMultiSelect = option.IsMultiSelect;
                mcqOption.AnswerOptions = option.AnswerOptions ?? mcqOption.AnswerOptions;
                options.Add(mcqOption.ToDto());
            }
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? mcqQuestion.OptionsToDto(options)
            : Error.Failure();
    }
}

public class UpdateMcqQuestionCommandValidator : AbstractValidator<UpdateMcqQuestionCommand>
{
    public UpdateMcqQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.Score).GreaterThan(0);

        RuleForEach(x => x.McqOptions).ChildRules(option =>
        {
            option.When(x => x.Id == Guid.Empty, () =>
            { 
              option.RuleFor(x => x.Option1).NotEmpty();

              option.RuleFor(x => x.Option2).NotEmpty();

              option.RuleFor(x => x.AnswerOptions).NotEmpty().Matches(@"^\d+(,\d+)*$");

               option.RuleFor(x => x.AnswerOptions)
               .Must((option, answerOptions) =>
               option.IsMultiSelect ? answerOptions.Contains(",") : !answerOptions.Contains(","));
            });
        });
    }
}