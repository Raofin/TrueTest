using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Examinations.Queries;

public record GetExamByIdQuery(Guid ExamId) : IRequest<ErrorOr<ExamResponse>>;

public class GetExamByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByIdQuery, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);

        return exam is null
            ? Error.NotFound()
            : exam.ToDto();
    }
}

public class GetExamByIdQueryValidator : AbstractValidator<GetExamByIdQuery>
{
    public GetExamByIdQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}