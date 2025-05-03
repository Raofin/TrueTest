using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Application.Features.Questions.ProblemSolving.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

/// <summary>
/// API endpoints for managing problem-solving questions.
/// </summary>
[Route("Questions/Problem")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class QuestionProblemController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a new problem-solving question.</summary>
    /// <param name="command">Details of the problem-solving question to be created, including test cases.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the newly created problem-solving question.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Creates a new problem-solving question.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateProblemSolvingAsync(CreateProblemSolvingCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific problem-solving question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the problem-solving question to retrieve.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the requested problem-solving question, including its test cases.</returns>
    [HttpGet("{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves a specific problem-solving question by its unique identifier.")]
    [ProducesResponseType<ProblemQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetProblemSolvingByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var query = new GetProblemSolvingByIdQuery(questionId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves all problem-solving questions associated with a specific exam.</summary>
    /// <param name="examId">The unique identifier of the exam.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of all problem-solving questions belonging to the specified exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Retrieves all problem-solving questions associated with a specific exam.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSolvingByExamAsync(Guid examId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProblemSolvingByExamQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates an existing problem-solving question with new details and test cases.</summary>
    /// <param name="command">The unique identifier of the problem-solving question to update and the new details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the updated problem-solving question.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Updates an existing problem-solving question with new details and test cases.")]
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

    /// <summary>Deletes a specific problem-solving question by its unique identifier.</summary>
    /// <param name="questionId">The unique identifier of the problem-solving question to delete.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating that the problem-solving question has been deleted.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes a specific problem-solving question by its unique identifier.")]
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

    /// <summary>Deletes a specific test case associated with a problem-solving question.</summary>
    /// <param name="testCaseId">The unique identifier of the test case to delete.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating that the test case has been deleted.</returns>
    [HttpDelete("TestCase/{testCaseId:guid}")]
    [HasPermission(ManageQuestions)]
    [EndpointDescription("Deletes a specific test case associated with a problem-solving question.")]
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