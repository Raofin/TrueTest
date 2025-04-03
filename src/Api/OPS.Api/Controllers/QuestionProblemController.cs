using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Application.Features.Questions.ProblemSolving.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Questions/Problem")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class QuestionProblemController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Creates a problem-solving question.</summary>
    /// <param name="command">A new problem-solving question with details.</param>
    /// <returns>Newly created problem-solving question.</returns>
    [HttpPost("Create")]
    [EndpointDescription("Creates a problem-solving question.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> CreateProblemSolvingAsync(CreateProblemSolvingCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific problem-solving question.</summary>
    /// <param name="questionId">Problem-solving question Id.</param>
    /// <returns>Problem-solving question with details.</returns>
    [HttpGet("{questionId:guid}")]
    [EndpointDescription("Retrieves a specific problem-solving question.")]
    [ProducesResponseType<ProblemQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetProblemSolvingByIdAsync(Guid questionId)
    {
        var query = new GetProblemSolvingByIdQuery(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves problem-solving questions of a specific exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>List of all problem-solving questions of a specific exam.</returns>
    [HttpGet("ByExam/{examId:guid}")]
    [EndpointDescription("Retrieves problem-solving questions of a specific exam.")]
    [ProducesResponseType<List<ProblemQuestionResponse>>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetProblemSolvingByExamAsync(Guid examId)
    {
        var query = new GetProblemSolvingByExamQuery(examId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Updates a problem-solving question.</summary>
    /// <param name="command">Problem-solving question Id and updated details.</param>
    /// <returns>The updated problem-solving question.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates a problem-solving question.")]
    [ProducesResponseType<ProblemQuestionResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> UpdateProblemSolvingAsync(UpdateProblemSolvingCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes a test case.</summary>
    /// <param name="testCaseId">Test case Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("TestCase/{testCaseId:guid}")]
    [EndpointDescription("Deletes a test case.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteTestCaseAsync(Guid testCaseId)
    {
        var query = new DeleteTestCaseCommand(testCaseId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Deletes a problem-solving question.</summary>
    /// <param name="questionId">Problem-solving question Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("Delete/{questionId:guid}")]
    [EndpointDescription("Deletes a problem-solving question.")]
    [ProducesResponseType<EmptyResult>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteProblemSolvingAsync(Guid questionId)
    {
        var query = new DeleteProblemSolvingCommand(questionId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}