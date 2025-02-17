using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Submit;
using OPS.Domain;

namespace OPS.Application.Features.WrittenSubmissions.Queries;

public record GetWrittenSubmissionByIdQuery(Guid WrittenSubmissionId) : IRequest<ErrorOr<WrittenSubmissionResponse>>;

public class GetWrittenSubmissionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenSubmissionByIdQuery, ErrorOr<WrittenSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<WrittenSubmissionResponse>> Handle(GetWrittenSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var writtenSubmission = await _unitOfWork.WrittenSubmission.GetAsync(request.WrittenSubmissionId, cancellationToken);

        return writtenSubmission is null
            ? Error.NotFound("WrittenSubmission not found.")
            : writtenSubmission.ToDto();
    }
}

public class GetWrittenSubmissionByIdQueryValidator : AbstractValidator<GetWrittenSubmissionByIdQuery>
{
    public GetWrittenSubmissionByIdQueryValidator()
    {
        RuleFor(x => x.WrittenSubmissionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}