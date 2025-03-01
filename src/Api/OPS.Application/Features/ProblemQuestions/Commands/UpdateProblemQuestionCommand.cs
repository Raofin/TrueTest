using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Features.ProblemQuestions.Commands;

public record UpdateProblemQuestionCommand(
    Guid QuestionId,
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    bool IsActive,
    List<TestCaseResponse> TestCases 
    ) : IRequest<ErrorOr<bool>>;

public class UpdateProblemQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProblemQuestionCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<bool>> Handle(UpdateProblemQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExaminationId, cancellationToken);
        var question = await _unitOfWork.Question.GetAsync(request.QuestionId, cancellationToken);
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

        return result > 0 ? true : Error.Failure();
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