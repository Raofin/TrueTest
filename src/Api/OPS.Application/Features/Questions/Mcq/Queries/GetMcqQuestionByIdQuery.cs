using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Mcq.Queries;

public record GetMcqQuestionByIdQuery(Guid QuestionId) : IRequest<ErrorOr<McqQuestionResponse>>;

public class GetMcqQuestionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqQuestionByIdQuery, ErrorOr<McqQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqQuestionResponse>> Handle(GetMcqQuestionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetWithMcqOption(request.QuestionId, cancellationToken);

        return questions is null
            ? Error.NotFound()
            : questions.ToMcqQuestionDto();
    }
}

public class GetMcqQuestionByIdQueryValidator : AbstractValidator<GetMcqQuestionByIdQuery>
{
    public GetMcqQuestionByIdQueryValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}