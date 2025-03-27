using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record GetExamByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<OngoingExamResponse>>;

public class GetExamByCandidateQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByCandidateQuery, ErrorOr<OngoingExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<OngoingExamResponse>> Handle(
        GetExamByCandidateQuery request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetWithAllQuesAndSubmission(
            request.ExamId, request.AccountId, cancellationToken);

        return exam is null
            ? Error.Unexpected(description: "Invalid ExamId")
            : exam.ToOngoingExamDto();
    }
}

public class GetExamByCandidateQueryValidator : AbstractValidator<GetExamByCandidateQuery>
{
    public GetExamByCandidateQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}