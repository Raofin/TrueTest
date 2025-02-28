using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Queries;

public record GetAllQuestionByExamIdQuestionTypeIdQuery(Guid ExamId,int QuestionTypeId) : IRequest<ErrorOr<List<QuestionResponse>>>;

public class GetAllQuestionByExamIdQuestionTypeIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllQuestionByExamIdQuestionTypeIdQuery, ErrorOr<List<QuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<QuestionResponse>>> Handle(GetAllQuestionByExamIdQuestionTypeIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllQuestionByExamIdQuestionTypeIdAsync(request.ExamId, request.QuestionTypeId, cancellationToken);

        return questions.Select(e => e.ToDto()).ToList();
    }
}

public class GetAllQuestionByExamIdQuestionTypeIdQueryValidator : AbstractValidator<GetAllQuestionByExamIdQuestionTypeIdQuery>
{
    public GetAllQuestionByExamIdQuestionTypeIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}