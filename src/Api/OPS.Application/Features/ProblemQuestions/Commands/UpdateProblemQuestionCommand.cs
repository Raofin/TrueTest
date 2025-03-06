using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Features.ProblemQuestions.Commands;

public record UpdateProblemQuestionCommand(
    Guid Id,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    bool IsActive,
    List<TestCaseResponse> TestCases 
    ) : IRequest<ErrorOr<ProblemQuestionResponse>>;

public class UpdateProblemQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProblemQuestionCommand, ErrorOr<ProblemQuestionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemQuestionResponse>> Handle(UpdateProblemQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExaminationId, cancellationToken);
        var question = await _unitOfWork.Question.GetAsync(request.Id, cancellationToken);
        if (examExists == null || question == null) return Error.NotFound();


        question.StatementMarkdown = request.StatementMarkdown ?? question.StatementMarkdown;
        question.Score = request.Score;
        question.DifficultyId = request.DifficultyId;  
        question.IsActive = request.IsActive;
        question.UpdatedAt = DateTime.UtcNow;



        foreach (var test in request.TestCases)
        {
            if (test.Input == null || test.Output == null)
            {
                return Error.NotFound();
            }

            if (test.Id != Guid.Empty)
            {
                var existingTestCase = await _unitOfWork.TestCase.GetAsync(test.Id, cancellationToken);
                if(existingTestCase == null) 
                    return Error.NotFound();    

                existingTestCase.Input = test.Input;
                existingTestCase.Output = test.Output;
                existingTestCase.UpdatedAt = DateTime.UtcNow;   
            }
            else
            {
                var newTestCase = new TestCase
                {
                    Input = test.Input,
                    Output = test.Output,
                    QuestionId = question.Id
                };
                _unitOfWork.TestCase.Add(newTestCase);
            }
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        var problemQuestion = new ProblemQuestionResponse(
            question.Id,
            question.StatementMarkdown,
            question.Score,
            question.ExaminationId,
            question.DifficultyId,
            question.QuestionTypeId,
            question.CreatedAt,
            question.UpdatedAt,
            question.IsActive,
            question.TestCases.Select(x => new TestCaseResponse(x.Id, question.Id, x.Input, x.Output)).ToList()
        );

        return result > 0
            ? problemQuestion
            : Error.Failure();
    }
}

public class UpdateProblemQuestionCommandValidator : AbstractValidator<UpdateProblemQuestionCommand>
{
    public UpdateProblemQuestionCommandValidator()
    {
        RuleFor(x => x.ExaminationId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.DifficultyId)
            .NotEmpty();
        RuleFor(x => x.QuestionTypeId)
            .NotEmpty();
        RuleFor(x => x.Score)
            .NotEmpty();
    }
}