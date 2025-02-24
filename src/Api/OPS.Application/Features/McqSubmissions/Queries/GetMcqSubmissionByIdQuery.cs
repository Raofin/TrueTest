using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.Submit;
using OPS.Domain;
using OPS.Domain.Entities.Submit;


namespace OPS.Application.Features.McqSubmissions.Queries;

public record GetMcqSubmissionByIdQuery(Guid McqSubmissionId) : IRequest<ErrorOr<McqSubmissionResponse>>;

public class GetMcqSubmissionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqSubmissionByIdQuery, ErrorOr<McqSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqSubmissionResponse>> Handle(GetMcqSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var mcqSubmission = await _unitOfWork.McqSubmission.GetAsync(request.McqSubmissionId, cancellationToken);

        return mcqSubmission is null
            ? Error.NotFound()
            : mcqSubmission.ToDto();
    }
}

public class GetMcqSubmissionByIdQueryValidator : AbstractValidator<GetMcqSubmissionByIdQuery>
{
    public GetMcqSubmissionByIdQueryValidator()
    {
        RuleFor(x => x.McqSubmissionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}