using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Mcq.Queries;

public record GetAllMcqByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<McqQuestionResponse>>>;

public class GetAllMcqByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllMcqByExamIdQuery, ErrorOr<List<McqQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqQuestionResponse>>> Handle(GetAllMcqByExamIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetMcqByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(q => q.ToMcqQuestionDto()).ToList();
    }
}

public class GetAllMcqByExamIdQueryValidator : AbstractValidator<GetAllMcqByExamIdQuery>
{
    public GetAllMcqByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}