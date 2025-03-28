using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record GetWrittenQuesWithSubmissionQuery(Guid ExamId, Guid AccountId)
    : IRequest<ErrorOr<List<WrittenQuesWithSubmissionResponse>>>;

public class GetWrittenQuesWithSubmissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetWrittenQuesWithSubmissionQuery, ErrorOr<List<WrittenQuesWithSubmissionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<WrittenQuesWithSubmissionResponse>>> Handle(
        GetWrittenQuesWithSubmissionQuery request, CancellationToken cancellationToken)
    {
        var questions = await _unitOfWork.WrittenSubmission
            .GetQuesWithSubmission(request.ExamId, request.AccountId, cancellationToken);

        return questions.Count == 0
            ? Error.Unexpected(description: "Invalid ExamId or AccountId")
            : questions.Select(q => q.ToWrittenWithSubmissionDto()).ToList();
    }
}

public class GetWrittenQuesWithSubmissionQueryValidator : AbstractValidator<GetWrittenQuesWithSubmissionQuery>
{
    public GetWrittenQuesWithSubmissionQueryValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}