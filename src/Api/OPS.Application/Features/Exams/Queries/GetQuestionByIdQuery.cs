using MediatR;
using ErrorOr;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Queries;

public record GetQuestionByIdQuery(Guid Id) : IRequest<ErrorOr<QuestionResponse>>;

public class GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetQuestionByIdQuery, ErrorOr<QuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<QuestionResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(request.Id, cancellationToken);

        return question is null
            ? Error.NotFound("Question not found.")
            : question.ToDto();
    }
}