using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Features.McqQuestions.Commands;

public record CreateMcqQuestionCommand(
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    DifficultyType DifficultyId,
    List<CreateMcqOptionResponse> McqOptions
    ) : IRequest<ErrorOr<McqQuestionResponse>>;

public class CreateMcqQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMcqQuestionCommand, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(CreateMcqQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExaminationId, cancellationToken);
        if (examExists == null) return Error.NotFound();     

        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Points = request.Score,
            DifficultyId = (int)request.DifficultyId,
            QuestionTypeId = 3,
            ExaminationId = request.ExaminationId
        };

        _unitOfWork.Question.Add(question);
        List<McqOptionResponse> options = new List<McqOptionResponse>();
       

        foreach (var option in request.McqOptions)
        {
            var mcqOption = new McqOption
            {
                Option1 = option.Option1,
                Option2 = option.Option2,
                Option3 = option.Option3,
                Option4 = option.Option4,
                IsMultiSelect = option.IsMultiSelect,
                AnswerOptions = option.AnswerOptions,   
                QuestionId = question.Id
            };  
            _unitOfWork.McqOption.Add(mcqOption);
            options.Add(mcqOption.ToDto());
        }
   
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? question.OptionsToDto(options)
            : Error.Failure();
    }
}

public class CreateMcqQuestionCommandValidator : AbstractValidator<CreateMcqQuestionCommand>
{
    public CreateMcqQuestionCommandValidator()
    {

        RuleFor(x => x.StatementMarkdown).NotEmpty();

        RuleFor(x => x.Score).GreaterThan(0);
        
        RuleFor(x => x.DifficultyId).NotEmpty();
        
        RuleFor(x => x.ExaminationId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleForEach(x => x.McqOptions).ChildRules(option =>
        {
            option.RuleFor(x => x.Option1).NotEmpty();
            
            option.RuleFor(x => x.Option2).NotEmpty();
            
            option.RuleFor(x => x.AnswerOptions).NotEmpty().Matches(@"^\d+(,\d+)*$");
            
            option.RuleFor(x => x.AnswerOptions)
            .Must((option, answerOptions) => 
            option.IsMultiSelect ? answerOptions.Contains(","): !answerOptions.Contains(","));
        });
    }
}