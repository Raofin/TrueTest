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

public record CreateProblemQuestionCommand(
    string StatementMarkdown,
    decimal Score,
    Guid ExaminationId,
    int DifficultyId,
    int QuestionTypeId,
    bool IsActive,
    List<TestCaseResponse> TestCases 
    ) : IRequest<ErrorOr<bool>>;

public class CreateProblemQuestionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProblemQuestionCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<bool>> Handle(CreateProblemQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var examExists = await _unitOfWork.Exam.GetAsync(request.ExaminationId, cancellationToken);
       
        if(examExists == null) return Error.NotFound();

        var question = new Question
        {
            StatementMarkdown = request.StatementMarkdown,
            Score = request.Score,
            ExaminationId = request.ExaminationId,
            DifficultyId = request.DifficultyId,
            QuestionTypeId = request.QuestionTypeId
        };

        _unitOfWork.Question.Add(question);

        foreach (var test in request.TestCases)
        {
            if (test.Input == null || test.Output == null)
            {
                return Error.NotFound();
            }

            var testCase = new TestCase
            {
                Input = test.Input, 
                Output = test.Output,
                QuestionId = question.Id
            };
            _unitOfWork.TestCase.Add(testCase);
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? true : Error.Failure();
    }
}

public class CreateProblemQuestionCommandValidator : AbstractValidator<CreateProblemQuestionCommand>
{
    public CreateProblemQuestionCommandValidator()
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