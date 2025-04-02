using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetExamByIdQuery(Guid ExamId) : IRequest<ErrorOr<ExamWithQuestionsResponse>>;

public class GetExamByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByIdQuery, ErrorOr<ExamWithQuestionsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamWithQuestionsResponse>> Handle(
        GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithQuestionsAsync(request.ExamId, cancellationToken);
        if (exam is null) return Error.NotFound();

        var response = new ExamWithQuestionsResponse(
            exam.MapToDto(),
            exam.MapToQuestionDto()
        );

        return response;
    }
}