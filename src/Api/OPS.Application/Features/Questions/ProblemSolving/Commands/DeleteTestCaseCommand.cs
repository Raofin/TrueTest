using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Questions.ProblemSolving.Commands;

public record DeleteTestCaseCommand(Guid TestCaseId) : IRequest<ErrorOr<Success>>;

public class DeleteTestCaseCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTestCaseCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteTestCaseCommand request, CancellationToken cancellationToken)
    {
        var testCase = await _unitOfWork.TestCase.GetAsync(request.TestCaseId, cancellationToken);
        if (testCase is null) return Error.NotFound();

        var isExamPublished = await _unitOfWork.Exam.IsPublished(questionId: testCase.QuestionId, cancellationToken);
        if (isExamPublished) return Error.Conflict(description: "Exam of this question is already published");

        _unitOfWork.TestCase.Remove(testCase);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class DeleteTestCaseCommandValidator : AbstractValidator<DeleteTestCaseCommand>
{
    public DeleteTestCaseCommandValidator()
    {
        RuleFor(x => x.TestCaseId)
            .IsValidGuid();
    }
}