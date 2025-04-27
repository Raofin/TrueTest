using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Application.Features.Questions.Mcq.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constents.Permissions;

namespace OPS.Api.Controllers;

[Route("Questions/Mcq")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class QuestionMcqController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates an MCQ Question.</summary>
    /// <param name="command">A new MCQ question with details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Newly created MCQ question.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Creates an MCQ Question.")]
    [ProducesResponseType<List<McqQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateMcqAsync(CreateMcqCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves an MCQ question.</summary>
    /// <param name="questionId">MCQ question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>MCQ question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves an MCQ question.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetMcqByIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var query = new GetMcqQuestionByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves MCQ questions of an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of all MCQ questions of an exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves MCQ questions of an exam.")]
    [ProducesResponseType<List<McqQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqByExamAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var query = new GetMcqByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates an MCQ question.</summary>
    /// <param name="command">MCQ question Id and updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated MCQ question.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Updates an MCQ question.")]
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

    /// <summary>Deletes an MCQ question.</summary>
    /// <param name="questionId">MCQ question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes an MCQ question.")]
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