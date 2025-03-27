using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Submissions.Commands;
using OPS.Application.Features.Submissions.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Submissions")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class SubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves MCQ submissions for a specific exam and user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <returns>List of MCQ submissions.</returns>
    [HttpGet("Mcq/ByExam/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves MCQ submissions for a specific exam and user.")]
    [ProducesResponseType<List<McqSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetMcqQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Creates or updates an MCQ submission.</summary>
    /// <param name="command">MCQ submission details.</param>
    /// <returns>The saved or updated MCQ submission.</returns>
    [HttpPost("Mcq/Save")]
    [EndpointDescription("Creates or updates an MCQ submission.")]
    [ProducesResponseType<McqSubmitResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveMcqSubmissionAsync(SaveMcqSubmissionCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Creates or updates a problem-solving submission.</summary>
    /// <param name="command">Problem-solving submission details.</param>
    /// <returns>The saved or updated problem-solving submission.</returns>
    [HttpPost("Problem/Save")]
    [EndpointDescription("Creates or updates a problem-solving submission.")]
    [ProducesResponseType<ProblemSubmitResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveProblemAsync(SaveProblemSubmissionCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves problem-solving submissions for a specific exam and user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">User Account Id.</param>
    /// <returns>List of problem-solving submissions.</returns>
    [HttpGet("Problem/ByExam/{examId:guid}/{accountId:guid}")]
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
    [HttpGet("Written/ByExam/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves written submissions for a specific exam and user.")]
    [ProducesResponseType<List<WrittenQuesWithSubmissionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Creates or updates a written submission.</summary>
    /// <param name="command">Written submission details.</param>
    /// <returns>The saved or updated written submission.</returns>
    [HttpPost("Written/Save")]
    [EndpointDescription("Creates or updates a written submission.")]
    [ProducesResponseType<WrittenSubmitResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveWrittenSubmissionAsync(SaveWrittenSubmissionCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }
}