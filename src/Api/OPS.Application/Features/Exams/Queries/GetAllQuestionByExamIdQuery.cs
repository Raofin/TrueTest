using MediatR;
using ErrorOr;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Queries;

public record GetAllQuestionByExamIdQuery(Guid Id) : IRequest<ErrorOr<List<QuestionResponse>>>;

public class GetAllQuestionByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllQuestionByExamIdQuery, ErrorOr<List<QuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<QuestionResponse>>> Handle(GetAllQuestionByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllQuestionByExamIdAsync(request.Id, cancellationToken);

        return questions.Select(e => e.ToDto()).ToList();
    }
}