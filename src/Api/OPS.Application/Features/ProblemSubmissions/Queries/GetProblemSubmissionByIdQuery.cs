using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;


namespace OPS.Application.Features.ProblemSubmissions.Queries;

public record GetProblemSubmissionByIdQuery(Guid ProblemSubmissionId) : IRequest<ErrorOr<ProblemSubmitResponse>>;

public class GetProblemSubmissionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProblemSubmissionByIdQuery, ErrorOr<ProblemSubmitResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProblemSubmitResponse>> Handle(GetProblemSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var ProblemSubmission = await _unitOfWork.ProblemSubmission.GetAsync(request.ProblemSubmissionId, cancellationToken);

        // return ProblemSubmission is null
        //     ? Error.NotFound()
        //     : ProblemSubmission.ToDto();
        
        return Error.Failure();

    }
}

public class GetProblemSubmissionByIdQueryValidator : AbstractValidator<GetProblemSubmissionByIdQuery>
{
    public GetProblemSubmissionByIdQueryValidator()
    {
        RuleFor(x => x.ProblemSubmissionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}