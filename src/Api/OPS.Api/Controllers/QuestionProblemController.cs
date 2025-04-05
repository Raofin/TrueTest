using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Application.Features.Questions.ProblemSolving.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("Questions/Problem")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class QuestionProblemController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a problem-solving question.</summary>
    /// <param name="command">A new problem-solving question with details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Newly created problem-solving question.</returns>
    [HttpPost("Create")]
    [EndpointDescription("Creates a problem-solving question.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateProblemSolvingAsync(CreateProblemSolvingCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a problem-solving question.</summary>
    /// <param name="questionId">Problem-solving question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Problem-solving question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [EndpointDescription("Retrieves a problem-solving question.")]
    [ProducesResponseType<ProblemQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetProblemSolvingByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var query = new GetProblemSolvingByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves problem-solving questions of an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of all problem-solving questions of an exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [EndpointDescription("Retrieves problem-solving questions of an exam.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSolvingByExamAsync(Guid examId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProblemSolvingByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates a problem-solving question.</summary>
    /// <param name="command">Problem-solving question Id and updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated problem-solving question.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates a problem-solving question.")]
    [ProducesResponseType<ProblemQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateProblemSolvingAsync(UpdateProblemSolvingCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a problem-solving question.</summary>
    /// <param name="questionId">Problem-solving question Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [EndpointDescription("Deletes a problem-solving question.")]
    [ProducesResponseType<EmptyResult>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> DeleteProblemSolvingAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var query = new DeleteProblemSolvingCommand(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a test case.</summary>
    /// <param name="testCaseId">Test case Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("TestCase/{testCaseId:guid}")]
    [EndpointDescription("Deletes a test case.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> DeleteTestCaseAsync(Guid testCaseId, CancellationToken cancellationToken)
    {
        var query = new DeleteTestCaseCommand(testCaseId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }
}