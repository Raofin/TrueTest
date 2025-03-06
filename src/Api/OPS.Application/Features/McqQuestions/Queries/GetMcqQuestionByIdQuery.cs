using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;


namespace OPS.Application.Features.McqQuestions.Queries;

public record GetMcqQuestionByIdQuery(Guid QuestionId) : IRequest<ErrorOr<McqQuestionResponse>>;

public class GetMcqQuestionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqQuestionByIdQuery, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(GetMcqQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);

        if (questions == null || questions.QuestionTypeId != 3)
        {
            return Error.NotFound();
        }

        var options = await _unitOfWork.McqOption.GetMcqOptionsByQuestionIdAsync(questions.Id, cancellationToken);
        var mcqQuestion = questions.OptionsToDto(options.Select(o => o.ToDto()).ToList());

        return mcqQuestion;
    }
}

public class GetMcqQuestionByIdQueryValidator : AbstractValidator<GetMcqQuestionByIdQuery>
{
    public GetMcqQuestionByIdQueryValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}