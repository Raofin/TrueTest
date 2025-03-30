using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Review.Queries;

public record GetExamByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamReviewResponse>>;

public class GetExamByCandidateQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByCandidateQuery, ErrorOr<ExamReviewResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamReviewResponse>> Handle(
        GetExamByCandidateQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _unitOfWork.Exam.GetCandidateAsync(
            request.ExamId, request.AccountId, cancellationToken);
        if (candidate is null) return Error.NotFound(description: "Candidate not found");

        var exam = await _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(
            request.ExamId, request.AccountId, cancellationToken);
        if (exam is null) return Error.NotFound(description: "Exam not found");

        var examReview = new ExamReviewResponse(
            exam.Id,
            exam.Title,
            exam.DurationMinutes,
            100,
            candidate.Account!.ToDto(),
            new ExamResultsResponse(
                100,
                candidate.StartedAt,
                candidate.SubmittedAt,
                candidate.HasCheated
            ),
            new QuestionsWithSubmissionResponse(
                exam.Questions.Select(q => q.ToProblemWithSubmissionDto()).ToList(),
                exam.Questions.Select(q => q.ToWrittenWithSubmissionDto()).ToList(),
                exam.Questions.Select(q => q.ToMcqWithSubmissionDto()).ToList()
            )
        );

        return examReview;
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