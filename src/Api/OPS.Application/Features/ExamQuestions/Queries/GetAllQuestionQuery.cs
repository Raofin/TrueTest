using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Queries;

public record GetAllQuestionsQuery : IRequest<ErrorOr<List<QuestionResponse>>>;

public class GetAllQuestionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllQuestionsQuery, ErrorOr<List<QuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<QuestionResponse>>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAsync(cancellationToken);

        return questions.Select(e => e.ToDto()).ToList();
    }
}