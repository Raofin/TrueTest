using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;


namespace OPS.Application.Features.ProblemQuestions.Queries;

public record GetProblemQuestionsByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<ProblemQuestionResponse>>>;

public class GetProblemQuestionsByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemQuestionsByExamIdQuery, ErrorOr<List<ProblemQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ProblemQuestionResponse>>> Handle(GetProblemQuestionsByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllQuestionByExamIdAsync(request.ExamId, cancellationToken);

        var result = new List<ProblemQuestionResponse>();

        foreach (var question in questions)
        {
            var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(question.Id, cancellationToken);

            var problemQuestionResponse = new ProblemQuestionResponse(
                Id: question.Id,
                StatementMarkdown: question.StatementMarkdown,
                Score: question.Score,
                ExaminationId: question.ExaminationId,
                DifficultyId: question.DifficultyId,
                QuestionTypeId: question.QuestionTypeId,
                CreatedAt: question.CreatedAt,
                UpdatedAt: question.UpdatedAt,
                IsActive: question.IsActive,
                TestCases: testCases.Select(tc => new TestCaseResponse(tc.Id, question.Id, tc.Input, tc.Output)).ToList()
            );

            result.Add(problemQuestionResponse);
        }

        return result;
    }
}

public class GetProblemQuestionsByExamIdQueryValidator : AbstractValidator<GetProblemQuestionsByExamIdQuery>
{
    public GetProblemQuestionsByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}