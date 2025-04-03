using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Application.Features.Questions.Written.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Questions/Written")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class QuestionWrittenController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a written question.</summary>
    /// <param name="command">A new written question with details.</param>
    /// <returns>Newly created written question.</returns>
    [HttpPost("Create")]
    [EndpointDescription("Creates a written question.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateWrittenAsync(CreateWrittenCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific written question.</summary>
    /// <param name="questionId">Written question Id.</param>
    /// <returns>Written question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [EndpointDescription("Retrieves a specific written question.")]
    [ProducesResponseType<WrittenQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetWrittenByIdAsync(Guid questionId)
    {
        var query = new GetWrittenByIdQuery(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves written questions of a specific exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>List of all written questions of a specific exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [EndpointDescription("Retrieves written questions of a specific exam.")]
    [ProducesResponseType<List<WrittenQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetWrittenByExamAsync(Guid examId)
    {
        var query = new GetAllWrittenByExamIdQuery(examId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Updates a written question.</summary>
    /// <param name="command">Written question Id and updated details.</param>
    /// <returns>The updated written question.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates a written question.")]
    [ProducesResponseType<WrittenQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> UpdateWrittenAsync(UpdateWrittenCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes a written question.</summary>
    /// <param name="questionId">Written question Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [EndpointDescription("Deletes a written question.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteWrittenAsync(Guid questionId)
    {
        var query = new DeleteWrittenCommand(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}