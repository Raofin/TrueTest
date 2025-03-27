using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Examinations.Commands;
using OPS.Application.Features.Examinations.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

// [AuthorizeRoles(RoleType.Admin)]
[Route("api/Exam")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all exams.</summary>
    /// <returns>List of all exams.</returns>
    [HttpGet("All")]
    [EndpointDescription("Retrieves all exams.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var query = new GetAllExamsQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific exam with all questions.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>Exam details.</returns>
    [HttpGet("{examId:guid}")]
    [EndpointDescription("Retrieves a specific exam with all questions.")]
    [ProducesResponseType<ExamWithQuestionsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId)
    {
        var query = new GetExamByIdQuery(examId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Creates a new exam.</summary>
    /// <param name="command">Exam with details.</param>
    /// <returns>Newly created exam details.</returns>
    [HttpPost("Create")]
    [EndpointDescription("Creates a new exam.")]
    [ProducesResponseType<ExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateExamAsync(CreateExamCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Updates an existing exam.</summary>
    /// <param name="command">Exam Id and the updated details.</param>
    /// <returns>Updated exam details.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates an existing exam.")]
    [ProducesResponseType<ExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(UpdateExamCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes an existing exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("Delete/{examId:guid}")]
    [EndpointDescription("Deletes an existing exam.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid examId)
    {
        var command = new DeleteExamCommand(examId);
        var response = await _mediator.Send(command);
        return ToResult(response);
    }
}