using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Review.Queries;

public record GetExamByCandidateQuery(Guid ExamId, Guid AccountId) : IRequest<ErrorOr<ExamReviewResponse>>;

public class GetExamByCandidateQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetExamByCandidateQuery, ErrorOr<ExamReviewResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamReviewResponse>> Handle(
        GetExamByCandidateQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Exam.GetWithAllQuesAndSubmission(
            request.ExamId, request.AccountId, cancellationToken);

        var candidate = account.ExamCandidates.First();
        var exam = candidate.Examination;

        var problemQuestionsWithSubmissions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .Select(q => q.ToQuesProblemSubmissionDto())
            .ToList();

        var writtenQuestionsWithSubmissions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.Written)
            .Select(q => q.ToWrittenWithSubmissionDto())
            .ToList();

        var mcqQuestionsWithSubmissions = exam.Questions
            .Where(q => q.QuestionTypeId == (int)QuestionType.MCQ)
            .Select(q => q.ToMcqWithSubmissionDto())
            .ToList();

        var examReview = new ExamReviewResponse(
            exam.Id,
            exam.Title,
            exam.DurationMinutes,
            100,
            account.ToDto(),
            new ExamResultsResponse(100, candidate.StartedAt, candidate.SubmittedAt, candidate.HasCheated),
            new QuestionsWithSubmission(
                problemQuestionsWithSubmissions,
                writtenQuestionsWithSubmissions,
                mcqQuestionsWithSubmissions
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