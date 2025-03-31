using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Enums;

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

        var problemQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .Select(q => q.MapToProblemQuestionDto())
            .ToList();

        var writtenQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
            .Select(q => q.MapToWrittenQuestionDto())
            .ToList();

        var mcqQuestions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
            .Select(q => q.MapToMcqQuestionDto())
            .ToList();

        var response = new ExamWithQuestionsResponse(
            exam.Id,
            exam.Title,
            exam.DescriptionMarkdown,
            exam.DurationMinutes,
            exam.Status(),
            exam.OpensAt,
            exam.ClosesAt,
            new QuestionResponses(
                problemQuestions,
                writtenQuestions,
                mcqQuestions
            )
        );

        return response;
    }
}