using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;


namespace OPS.Application.Features.ProblemQuestions.Queries;

public record GetProblemQuestionByIdQuery(Guid ProblemQuestionId) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class GetProblemQuestionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemQuestionByIdQuery, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(GetProblemQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAsync(request.ProblemQuestionId, cancellationToken);
        
        if(question == null) return Error.NotFound();

        var testCases = await _unitOfWork.TestCase.GetByQuestionIdAsync(question.Id, cancellationToken);

        var result = new ProblemQuestionResponse(
            Id: question.Id,
            StatementMarkdown: question.StatementMarkdown,
            Score: question.Score,
            ExaminationId: question.ExaminationId, 
            DifficultyId: question.DifficultyId,
            QuestionTypeId: question.QuestionTypeId,
            CreatedAt: question.CreatedAt,
            UpdatedAt: question.UpdatedAt,
            IsActive: question.IsActive,
            TestCases: testCases.Select(x => new TestCaseResponse(x.Id, question.Id, x.Input, x.Output)).ToList() 
        );

        return result;
    }
}

public class GetProblemQuestionByIdQueryValidator : AbstractValidator<GetProblemQuestionByIdQuery>
{
    public GetProblemQuestionByIdQueryValidator()
    {
        RuleFor(x => x.ProblemQuestionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}