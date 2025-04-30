using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Review.Commands;
using OPS.Application.Features.Review.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

/// <summary>
/// API endpoints for reviewing exam submissions and results.
/// </summary>
[Route("Review")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class ReviewController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves a list of all candidates who participated in a specific exam, along with their overall results.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of candidates and their aggregated results for the specified exam.</returns>
    [HttpGet("Candidates/{examId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves all candidates who participated in a specific exam, along with their overall results.")]
    [ProducesResponseType<List<CandidateResultResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetCandidatesByExamAsync(Guid examId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCandidatesByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves the detailed exam results for a specific candidate in a specific exam.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="accountId">The unique identifier of the candidate's account.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The detailed exam results for the specified candidate in the specified exam.</returns>
    [HttpGet("Candidates/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves the detailed exam results for a specific candidate in a specific exam.")]
    [ProducesResponseType<List<ExamResultResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetResultsByExamAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetResultByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Reviews a specific problem submission, allowing for feedback and scoring.</summary>
    /// <param name="command">Details for reviewing the problem submission, including the submission ID and review comments/scores.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Result indicating the success or failure of the review operation.</returns>
    [HttpPatch("Submission/Problem")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Reviews a specific problem submission, allowing for feedback and scoring.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewProblemSubmissionAsync(ReviewProblemCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Reviews a specific written submission, allowing for feedback and scoring.</summary>
    /// <param name="command">Details for reviewing the written submission, including the submission ID and review comments/scores.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Result indicating the success or failure of the review operation.</returns>
    [HttpPatch("Submission/Written")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Reviews a specific written submission, allowing for feedback and scoring.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewWrittenSubmissionAsync(ReviewWrittenCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>[Obsolete] Retrieves an exam along with all its questions and the submissions of a specific candidate.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="accountId">The unique identifier of the candidate's account.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the exam, its questions, and the submissions made by the specified candidate.</returns>
    [Obsolete("This endpoint is deprecated and will be removed in a future version.")]
    [HttpGet("Exam/QuestionsWithSubmission/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("[Obsolete] Retrieves an exam with questions and submissions of a candidate.")]
    [ProducesResponseType<ExamQuesWithSubmissionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetExamByCandidateAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetExamByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>[Obsolete] Retrieves all problem-solving submissions for a specific exam by a specific user.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="accountId">The unique identifier of the user's account.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of problem-solving submissions made by the specified user for the specified exam.</returns>
    [Obsolete("This endpoint is deprecated and will be removed in a future version.")]
    [HttpGet("Problem/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("[Obsolete] Retrieves problem-solving submissions for an exam of a user.")]
    [ProducesResponseType<List<ProblemQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSubmissionsByExamAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetProblemQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>[Obsolete] Retrieves all written submissions for a specific exam by a specific user.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="accountId">The unique identifier of the user's account.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of written submissions made by the specified user for the specified exam.</returns>
    [Obsolete("This endpoint is deprecated and will be removed in a future version.")]
    [HttpGet("Written/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("[Obsolete] Retrieves written submissions for an exam of a user.")]
    [ProducesResponseType<List<WrittenQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenSubmissionsAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>[Obsolete] Retrieves all MCQ submissions for a specific exam by a specific user.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="accountId">The unique identifier of the user's account.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of MCQ submissions made by the specified user for the specified exam.</returns>
    [Obsolete("This endpoint is deprecated and will be removed in a future version.")]
    [HttpGet("Mcq/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("[Obsolete] Retrieves MCQ submissions for an exam of a user.")]
    [ProducesResponseType<List<McqSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqSubmissionsAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetMcqQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }
}