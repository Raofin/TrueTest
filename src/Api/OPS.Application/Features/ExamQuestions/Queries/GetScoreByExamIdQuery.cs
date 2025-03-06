using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.ExamQuestions.Queries;

public record GetScoreByExamsIdQuery(Guid ExamId) : IRequest<ErrorOr<decimal>>;

public class GetScoreByExamsIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetScoreByExamsIdQuery, ErrorOr<decimal>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<decimal>> Handle(GetScoreByExamsIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Question.GetAllQuestionByExamIdAsync(request.ExamId, cancellationToken);
        var score = question.Sum(x => x.Score);

        return score;
    }
}

public class GetScoreByExamsIdQueryValidator : AbstractValidator<GetScoreByExamsIdQuery>
{
    public GetScoreByExamsIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}