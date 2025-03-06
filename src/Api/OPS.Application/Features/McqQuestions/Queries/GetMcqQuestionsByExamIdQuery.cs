using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;


namespace OPS.Application.Features.McqQuestions.Queries;

public record GetMcqQuestionsByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<McqQuestionResponse>>>;

public class GetMcqQuestionsByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqQuestionsByExamIdQuery, ErrorOr<List<McqQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqQuestionResponse>>> Handle(GetMcqQuestionsByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetAllQuestionByExamIdAsync(request.ExamId, cancellationToken);

        questions = questions.Where(q => q.QuestionTypeId == 3).ToList();

        var mcqQuestions = questions
            .Where(q => q.QuestionTypeId == 3)
            .Select(q => q.OptionsToDto(
                _unitOfWork.McqOption.GetMcqOptionsByQuestionIdAsync(q.Id, cancellationToken)
                    .Result.Select(o => o.ToDto()).ToList()
            ))
            .ToList();

        return mcqQuestions;
    }
}

public class GetMcqQuestionsByExamIdQueryValidator : AbstractValidator<GetMcqQuestionsByExamIdQuery>
{
    public GetMcqQuestionsByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}