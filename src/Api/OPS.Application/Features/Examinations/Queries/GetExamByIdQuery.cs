using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Examinations.Queries;

public record GetExamByIdQuery(Guid ExamId) : IRequest<ErrorOr<ExamWithQuestionsResponse>>;

public class GetExamByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByIdQuery, ErrorOr<ExamWithQuestionsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamWithQuestionsResponse>> Handle(
        GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuestionsAsync(request.ExamId, cancellationToken);

        return exam is null
            ? Error.NotFound()
            : exam.ToDtoWithQuestions();
    }
}