using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Features.McqOptions.Commands;

public record CreateMcqOptionCommand(
    Guid QuestionId,
    string Option1,
    string Option2,
    string Option3,
    string Option4,
    bool isMultiSelect,
    string AnswerOptions
    ) : IRequest<ErrorOr<McqOptionResponse>>;

public class CreateMcqOptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMcqOptionCommand, ErrorOr<McqOptionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqOptionResponse>> Handle(CreateMcqOptionCommand request,
        CancellationToken cancellationToken)
    {
        var questionExists = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
        if (questionExists == null) return Error.NotFound();       

        var mcqOption = new McqOption
        {
            QuestionId = request.QuestionId,   
            Option1 = request.Option1,
            Option2 = request.Option2,  
            Option3 = request.Option3,
            Option4 = request.Option4,
            IsMultiSelect = request.isMultiSelect,
            AnswerOptions = request.AnswerOptions
        };

        _unitOfWork.McqOption.Add(mcqOption);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? mcqOption.ToDto()
            : Error.Failure();
    }
}

public class CreateMcqOptionCommandValidator : AbstractValidator<CreateMcqOptionCommand>
{
    public CreateMcqOptionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.Option1).NotEmpty();
        RuleFor(x => x.Option2).NotEmpty();
        RuleFor(x => x.Option3).NotEmpty();
        RuleFor(x => x.Option4).NotEmpty();
        RuleFor(x => x.AnswerOptions).NotEmpty();
    }
}