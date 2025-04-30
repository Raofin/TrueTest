using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Application.Features.Questions.Mcq.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

/// <summary>
/// API endpoints for managing Multiple Choice Questions (MCQ).
/// </summary>
[Route("Questions/Mcq")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class QuestionMcqController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a new MCQ Question.</summary>
    /// <param name="command">Details of the MCQ question to be created, including options and correct answer.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the newly created MCQ question.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Creates a new MCQ Question.")]
    [ProducesResponseType<List<McqQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateMcqAsync(CreateMcqCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific MCQ question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the MCQ question to retrieve.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the requested MCQ question.</returns>
    [HttpGet("{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves a specific MCQ question by its unique identifier.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetMcqByIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var query = new GetMcqQuestionByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves all MCQ questions associated with a specific exam.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of all MCQ questions belonging to the specified exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves all MCQ questions associated with a specific exam.")]
    [ProducesResponseType<List<McqQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqByExamAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var query = new GetMcqByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates an existing MCQ question with new details.</summary>
    /// <param name="command">The unique identifier of the MCQ question to update and the new details, including options and correct answer.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the updated MCQ question.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Updates an existing MCQ question with new details.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(UpdateMcqCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a specific MCQ question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the MCQ question to delete.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating that the MCQ question has been deleted.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes a specific MCQ question by its unique identifier.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> DeleteAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var query = new DeleteMcqCommand(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }
}