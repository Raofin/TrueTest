using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Mcq.Queries;

public record GetMcqByExamQuery(Guid ExamId) : IRequest<ErrorOr<List<McqQuestionResponse>>>;

public class GetMcqByExamQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqByExamQuery, ErrorOr<List<McqQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<McqQuestionResponse>>> Handle(GetMcqByExamQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetMcqByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(q => q.MapToMcqQuestionDto()).ToList();
    }
}

public class GetMcqByExamQueryValidator : AbstractValidator<GetMcqByExamQuery>
{
    public GetMcqByExamQueryValidator()
    {
        RuleFor(x => x.ExamId).IsValidGuid();
    }
}