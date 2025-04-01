using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Review.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Review")]
public class ReviewController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves an exam with questions and submissions of a candidate.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id of a candidate.</param>
    /// <returns>Exam with questions and submissions.</returns>
    [HttpGet("Exam/QuestionsWithSubmission/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves an exam with questions and submissions of a candidate.")]
    [ProducesResponseType<ExamQuesWithSubmissionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetExamByCandidateAsync(Guid examId, Guid accountId)
    {
        var query = new GetExamByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves all submissions of a candidate for a specific exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id of a candidate.</param>
    /// <returns>Submissions of a candidate for a specific exam</returns>
    [HttpGet("Submissions/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves all submissions of a candidate for a specific exam.")]
    [ProducesResponseType<ExamSubmissionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetSubmissionsQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves exam results of a candidates.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id.</param>
    /// <returns>Candidate exam result.</returns>
    [HttpGet("Candidate/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves exam results of a candidates.")]
    [ProducesResponseType<List<CandidateResultResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetResultsByExamAsync(Guid examId, Guid accountId)
    {
        var query = new GetResultByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves problem-solving submissions for a specific exam and user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <returns>List of problem-solving submissions.</returns>
    [HttpGet("Problem/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves problem-solving submissions for a specific exam and user.")]
    [ProducesResponseType<List<ProblemQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSubmissionsByExamAsync(Guid examId, Guid accountId)
    {
        var query = new GetProblemQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves written submissions for a specific exam and user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <returns>List of written submissions.</returns>
    [HttpGet("Written/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves written submissions for a specific exam and user.")]
    [ProducesResponseType<List<WrittenQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves MCQ submissions for a specific exam and user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <returns>List of MCQ submissions.</returns>
    [HttpGet("Mcq/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves MCQ submissions for a specific exam and user.")]
    [ProducesResponseType<List<McqSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetMcqQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}