using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Written.Queries;

public record GetWrittenByIdQuery(Guid Id) : IRequest<ErrorOr<WrittenQuestionResponse>>;

public class GetWrittenByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenByIdQuery, ErrorOr<WrittenQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenQuestionResponse>> Handle(GetWrittenByIdQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetWrittenByIdAsync(request.Id, cancellationToken);

        return question is null
            ? Error.NotFound()
            : question.ToWrittenQuestionDto();
    }
}

public class GetWrittenByIdQueryValidator : AbstractValidator<GetWrittenByIdQuery>
{
    public GetWrittenByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}