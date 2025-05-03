using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Features.Candidates.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

/// <summary>
/// API endpoints for candidate-specific exam operations.
/// </summary>
[Route("Candidate")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class CandidateController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all exams of the authenticated user.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of exams with their results for the authenticated user.</returns>
    [HttpGet("Exams")]
    [HasPermission(AccessOwnExams)]
    [EndpointDescription("Retrieves all exams of the authenticated user, including their results.")]
    [ProducesResponseType<List<ExamWithResultResponse>>(Status200OK)]
    public async Task<IActionResult> GetExamsAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllExamsByCandidateQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Starts an exam for the authenticated user.</summary>
    /// <param name="examId">Identifier of the exam to start.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the exam, its questions, and any previously submitted answers.</returns>
    [HttpPost("Exam/Start/{examId:guid}")]
    [HasPermission(AccessOwnExams)]
    [EndpointDescription("Starts a specific exam for the authenticated user, providing exam details, questions, and prior submissions.")]
    [ProducesResponseType<ExamStartResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> StartExamAsync(Guid examId, CancellationToken cancellationToken)
    {
        var command = new StartExamCommand(examId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates or updates a problem-solving submission for the authenticated user.</summary>
    /// <param name="command">Details of the problem-solving submission to save or update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of test code execution results after saving or updating the problem-solving submission.</returns>
    [HttpPut("Submit/Problem/Save")]
    [HasPermission(SubmitAnswers)]
    [EndpointDescription("Creates or updates a problem-solving submission and returns the results of test code execution.")]
    [ProducesResponseType<List<TestCodeResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ExceptionResponse>(Status403Forbidden)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveProblemAsync(SaveProblemSubmissionsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates or updates a written submission for the authenticated user.</summary>
    /// <param name="command">Details of the written submission to save or update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Confirmation of the saved or updated written submission.</returns>
    [HttpPut("Submit/Written/Save")]
    [HasPermission(SubmitAnswers)]
    [EndpointDescription("Creates or updates a written submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ExceptionResponse>(Status403Forbidden)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveWrittenSubmissionAsync(SaveWrittenSubmissionsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates or updates an MCQ submission for the authenticated user.</summary>
    /// <param name="command">Details of the MCQ submission to save or update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Confirmation of the saved or updated MCQ submission.</returns>
    [HttpPut("Submit/Mcq/Save")]
    [HasPermission(SubmitAnswers)]
    [EndpointDescription("Creates or updates a multiple-choice question (MCQ) submission.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ExceptionResponse>(Status403Forbidden)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> SaveMcqSubmissionAsync(SaveMcqSubmissionsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    // submit an exam
    /// <summary>Submits the completed exam for the authenticated user.</summary>
    /// <param name="examId">Identifier of the exam to submit.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Result of the exam submission.</returns>
    [HttpPatch("Submit/Exam/{examId:guid}")]
    [HasPermission(SubmitAnswers)]
    [EndpointDescription("Submits the completed exam for the authenticated user.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> SubmitExamAsync(Guid examId, CancellationToken cancellationToken)
    {
        var command = new SubmitExamCommand(examId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Executes and tests code for a given problem-solving question.</summary>
    /// <param name="command">Code to execute along with the identifier of the question to test against.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Results of the code execution against the test cases.</returns>
    [AllowAnonymous]
    [HttpPost("RunAnyCode")]
    [EndpointDescription("Executes and tests code for a given problem-solving question.")]
    [ProducesResponseType<CodeRunResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> RunAnyCodeAsync(CodeRunCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}