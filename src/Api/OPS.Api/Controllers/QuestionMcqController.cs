using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Application.Features.Questions.Mcq.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Questions/Mcq")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class QuestionMcqController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates an MCQ Question.</summary>
    /// <param name="command">A new MCQ question with details.</param>
    /// <returns>Newly created MCQ question.</returns>
    [EndpointDescription("Creates an MCQ Question.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateMcqAsync(CreateMcqCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific MCQ question.</summary>
    /// <param name="questionId">MCQ question Id.</param>
    /// <returns>MCQ question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [EndpointDescription("Retrieves a specific MCQ question.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetMcqByIdAsync(Guid questionId)
    {
        var query = new GetMcqQuestionByIdQuery(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves MCQ questions of a specific exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>List of all MCQ questions of a specific exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [EndpointDescription("Retrieves MCQ questions of a specific exam.")]
    [ProducesResponseType<List<McqQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetMcqByExamAsync(Guid examId)
    {
        var query = new GetMcqByExamQuery(examId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Updates an MCQ question.</summary>
    /// <param name="command">MCQ question Id and updated details.</param>
    /// <returns>The updated MCQ question.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates an MCQ question.")]
    [ProducesResponseType<McqQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(UpdateMcqCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes an MCQ question.</summary>
    /// <param name="questionId">MCQ question Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [EndpointDescription("Deletes an MCQ question.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid questionId)
    {
        var query = new DeleteMcqCommand(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}