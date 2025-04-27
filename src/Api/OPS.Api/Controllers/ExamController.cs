using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Exams.Commands;
using OPS.Application.Features.Exams.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constents.Permissions;

namespace OPS.Api.Controllers;

// [AuthorizeRoles(RoleType.Admin)]
[Route("Exam")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all exams.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>List of all exams.</returns>
    [HttpGet]
    [HasPermission(ViewExams)]
    [EndpointDescription("Retrieves all exams.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetAllExamsAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllExamsQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves an exam with all questions.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Exam details.</returns>
    [HttpGet("{examId:guid}")]
    [HasPermission(ViewExams)]
    [EndpointDescription("Retrieves an exam with all questions.")]
    [ProducesResponseType<ExamWithQuestionsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var query = new GetExamByIdQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates a new exam.</summary>
    /// <param name="command">Exam with details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Newly created exam details.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Creates a new exam.")]
    [ProducesResponseType<ExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateExamAsync(CreateExamCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates an exam.</summary>
    /// <param name="command">Exam Id and the updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated exam details.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Updates an exam.")]
    [ProducesResponseType<ExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(UpdateExamCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Publishes an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpPost("Publish")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Publishes an exam.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> PublishAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var command = new PublishExamCommand(examId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Invites candidates to an exam.</summary>
    /// <param name="command">Exam Id and a list of emails.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpPost("Invite/Candidates")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Invites candidates to an exam.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> InviteCandidatesAsync(InviteCandidatesCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes an exam.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("Delete/{examId:guid}")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Deletes an exam.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> DeleteAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteExamCommand(examId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }
}