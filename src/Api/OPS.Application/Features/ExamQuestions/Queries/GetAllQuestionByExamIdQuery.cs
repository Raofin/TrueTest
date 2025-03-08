using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Queries;

public record GetAllQuestionByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<QuestionResponse>>>;

public class GetAllQuestionByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllQuestionByExamIdQuery, ErrorOr<List<QuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<QuestionResponse>>> Handle(GetAllQuestionByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(e => e.ToDto()).ToList();
    }
}

public class GetAllQuestionByExamIdQueryValidator : AbstractValidator<GetAllQuestionByExamIdQuery>
{
    public GetAllQuestionByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}