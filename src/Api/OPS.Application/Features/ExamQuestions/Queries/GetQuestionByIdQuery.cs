using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Queries;

public record GetQuestionByIdQuery(Guid QuestionId) : IRequest<ErrorOr<QuestionResponse>>;

public class GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetQuestionByIdQuery, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);

        return question is null
            ? Error.NotFound()
            : question.ToDto();
    }
}

public class GetQuestionByIdQueryValidator : AbstractValidator<GetQuestionByIdQuery>
{
    public GetQuestionByIdQueryValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}