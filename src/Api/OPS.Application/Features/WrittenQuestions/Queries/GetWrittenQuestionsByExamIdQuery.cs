using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;


namespace OPS.Application.Features.WrittenQuestions.Queries;

public record GetWrittenQuestionsByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<WrittenQuestionResponse>>>;

public class GetWrittenQuestionsByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenQuestionsByExamIdQuery, ErrorOr<List<WrittenQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenQuestionResponse>>> Handle(GetWrittenQuestionsByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllQuestionByExamIdAsync(request.ExamId, cancellationToken);

        questions = questions.Where(q => q.QuestionTypeId == 2).ToList();
        var result = new List<WrittenQuestionResponse>();

        foreach (var question in questions)
        {
            var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(question.Id, cancellationToken);

            var WrittenQuestionResponse = new WrittenQuestionResponse(
                Id: question.Id,
                HasLongAnswer: question.HasLongAnswer,
                StatementMarkdown: question.StatementMarkdown,
                Score: question.Score,
                ExaminationId: question.ExaminationId,
                DifficultyId: question.DifficultyId,
                QuestionTypeId: question.QuestionTypeId,
                CreatedAt: question.CreatedAt,
                UpdatedAt: question.UpdatedAt,
                IsActive: question.IsActive
            );

            result.Add(WrittenQuestionResponse);
        }

        return result;
    }
}

public class GetWrittenQuestionsByExamIdQueryValidator : AbstractValidator<GetWrittenQuestionsByExamIdQuery>
{
    public GetWrittenQuestionsByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}