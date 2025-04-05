using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Application.Features.Questions.Written.Queries;
using OPS.Infrastructure.Authentication.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Infrastructure.Authentication.Permission.Permissions;

namespace OPS.Api.Controllers;

[Route("Questions/Written")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class QuestionWrittenController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a written question.</summary>
    /// <param name="command">A new written question with details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Newly created written question.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Creates a written question.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateWrittenAsync(CreateWrittenCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a written question.</summary>
    /// <param name="questionId">Written question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Written question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves a written question.")]
    [ProducesResponseType<WrittenQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetWrittenByIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var query = new GetWrittenByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves written questions of an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of all written questions of an exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves written questions of an exam.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenByExamAsync(Guid examId, CancellationToken cancellationToken)
    {
        var query = new GetAllWrittenByExamIdQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates a written question.</summary>
    /// <param name="command">Written question Id and updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated written question.</returns>
    [HttpPut("Update")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Updates a written question.")]
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

    /// <summary>Deletes a written question.</summary>
    /// <param name="questionId">Written question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes a written question.")]
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