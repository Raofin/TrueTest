using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Features.Candidates.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Candidate")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class CandidateController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all exams of the authenticated user.</summary>
    /// <returns>List of exams by user.</returns>
    [HttpGet("Exams")]
    [EndpointDescription("Retrieves all exams of the authenticated user.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetExamsAsync()
    {
        var query = new GetAllExamsByCandidateQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Starts an exam for the authenticated user.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>Exam details, questions, and submitted answers.</returns>
    [HttpPost("Exam/Start/{examId:guid}")]
    [EndpointDescription("Starts an exam for the authenticated user.")]
    [ProducesResponseType<ExamStartResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> StartExamAsync(Guid examId)
    {
        var command = new StartExamCommand(examId);
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Creates or updates a problem-solving submission.</summary>
    /// <param name="command">Problem-solving submission details.</param>
    /// <returns>The saved or updated problem-solving submission.</returns>
    [HttpPost("Submit/Problem/Save")]
    [EndpointDescription("Creates or updates a problem-solving submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveProblemAsync(SaveProblemSubmissionsCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Creates or updates a written submission.</summary>
    /// <param name="command">Written submission details.</param>
    /// <returns>The saved or updated written submission.</returns>
    [HttpPost("Submit/Written/Save")]
    [EndpointDescription("Creates or updates a written submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveWrittenSubmissionAsync(SaveWrittenSubmissionsCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Creates or updates an MCQ submission.</summary>
    /// <param name="command">MCQ submission details.</param>
    /// <returns>The saved or updated MCQ submission.</returns>
    [HttpPost("Submit/Mcq/Save")]
    [EndpointDescription("Creates or updates an MCQ submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveMcqSubmissionAsync(SaveMcqSubmissionsCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Tests the code for a specific question.</summary>
    /// <param name="command">Code with the Question Id to test</param>
    /// <returns>Test results</returns>
    [HttpPost("TestCode")]
    [EndpointDescription("Tests the code for a specific question.")]
    [ProducesResponseType<List<TestCodeResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> TestCodeAsync(TestCodeCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }
}