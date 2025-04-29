using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Written.Queries;

public record GetWrittenByIdQuery(Guid QuestionId) : IRequest<ErrorOr<WrittenQuestionResponse>>;

public class GetWrittenByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenByIdQuery, ErrorOr<WrittenQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenQuestionResponse>> Handle(GetWrittenByIdQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWrittenByIdAsync(request.QuestionId, cancellationToken);

        return question is null
            ? Error.NotFound()
            : question.MapToWrittenQuestionDto();
    }
}

public class GetWrittenByIdQueryValidator : AbstractValidator<GetWrittenByIdQuery>
{
    public GetWrittenByIdQueryValidator()
    {
        RuleFor(x => x.QuestionId).IsValidGuid();
    }
}