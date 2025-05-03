using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Application.Features.Questions.Written.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

/// <summary>
/// API endpoints for managing written questions.
/// </summary>
[Route("Questions/Written")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class QuestionWrittenController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a new written question.</summary>
    /// <param name="command">Details of the written question to be created.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the newly created written question.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Creates a new written question.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateWrittenAsync(CreateWrittenCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific written question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the written question to retrieve.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the requested written question.</returns>
    [HttpGet("{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves a specific written question by its unique identifier.")]
    [ProducesResponseType<WrittenQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetWrittenByIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var query = new GetWrittenByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves all written questions associated with a specific exam.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of all written questions belonging to the specified exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves all written questions associated with a specific exam.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenByExamAsync(Guid examId, CancellationToken cancellationToken)
    {
        var query = new GetAllWrittenByExamIdQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates an existing written question with new details.</summary>
    /// <param name="command">The unique identifier of the written question to update and the new details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the updated written question.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Updates an existing written question with new details.")]
    [ProducesResponseType<WrittenQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateWrittenAsync(UpdateWrittenCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a specific written question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the written question to delete.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating that the written question has been deleted.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes a specific written question by its unique identifier.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> DeleteWrittenAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var query = new DeleteWrittenCommand(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }
}