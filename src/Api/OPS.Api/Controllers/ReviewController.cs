using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Review.Commands;
using OPS.Application.Features.Review.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constents.Permissions;

namespace OPS.Api.Controllers;

[Route("Review")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class ReviewController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all candidates with results of an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of candidates with results.</returns>
    [HttpGet("Candidates/{examId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves all candidates with results of an exam.")]
    [ProducesResponseType<List<CandidateResultResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetCandidatesByExamAsync(Guid examId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCandidatesByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves exam results of a list of candidates.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Candidate exam result.</returns>
    [HttpGet("Candidates/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves exam results of a list of candidates.")]
    [ProducesResponseType<List<ExamResultResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetResultsByExamAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetResultByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Reviews a problem submission.</summary>
    /// <param name="command">Problem submission id and review details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Result of the review operation.</returns>
    [HttpPatch("Submission/Problem")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Review a problem submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewProblemSubmissionAsync(ReviewProblemCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Reviews a written submission.</summary>
    /// <param name="command">Written submission id and review details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Result of the review operation.</returns>
    [HttpPatch("Submission/Written")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Review a written submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewWrittenSubmissionAsync(ReviewWrittenCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves an exam with questions and submissions of a candidate.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id of a candidate.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Exam with questions and submissions.</returns>
    [Obsolete]
    [HttpGet("Exam/QuestionsWithSubmission/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves an exam with questions and submissions of a candidate.")]
    [ProducesResponseType<ExamQuesWithSubmissionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetExamByCandidateAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetExamByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves problem-solving submissions for an exam of a user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of problem-solving submissions.</returns>
    [Obsolete]
    [HttpGet("Problem/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves problem-solving submissions for an exam of a user.")]
    [ProducesResponseType<List<ProblemQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSubmissionsByExamAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetProblemQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves written submissions for an exam of a user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of written submissions.</returns>
    [Obsolete]
    [HttpGet("Written/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves written submissions for an exam of a user.")]
    [ProducesResponseType<List<WrittenQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenSubmissionsAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves MCQ submissions for an exam of a user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of MCQ submissions.</returns>
    [Obsolete]
    [HttpGet("Mcq/{examId:guid}/{accountId:guid}")]
    [HasPermission(ReviewSubmission)]
    [EndpointDescription("Retrieves MCQ submissions for an exam of a user.")]
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