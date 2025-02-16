using MediatR;
using ErrorOr;
using FluentValidation;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Queries;

public record GetExamByIdQuery(Guid Id) : IRequest<ErrorOr<ExamResponse>>;

public class GetExamByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByIdQuery, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.Id, cancellationToken);

        return exam is null
            ? Error.NotFound()
            : exam.ToDto();
    }
}

public class GetExamByIdQueryValidator : AbstractValidator<GetExamByIdQuery>
{
    public GetExamByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => Guid.TryParse(id.ToString(), out _));
    }
}