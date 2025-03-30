using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Questions.Written.Queries;

public record GetAllWrittenByExamIdQuery(Guid ExamId) : IRequest<ErrorOr<List<WrittenQuestionResponse>>>;

public class GetAllWrittenByExamIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllWrittenByExamIdQuery, ErrorOr<List<WrittenQuestionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenQuestionResponse>>> Handle(GetAllWrittenByExamIdQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.Question.GetWrittenByExamIdAsync(request.ExamId, cancellationToken);

        return questions.Select(q => q.ToWrittenQuestionDto()).ToList();
    }
}

public class GetAllWrittenByExamIdQueryValidator : AbstractValidator<GetAllWrittenByExamIdQuery>
{
    public GetAllWrittenByExamIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}