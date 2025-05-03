using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Exams.Commands;
using OPS.Application.Features.Exams.Queries;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

[Route("Exam")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves a list of all exams.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list containing details of all exams.</returns>
    [HttpGet]
    [HasPermission(ViewExams)]
    [EndpointDescription("Retrieves a list of all exams.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetAllExamsAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllExamsQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves a specific exam along with all its associated questions.</summary>
    /// <param name="examId">The unique identifier of the exam to retrieve.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Detailed information about the requested exam, including its questions.</returns>
    [HttpGet("{examId:guid}")]
    [HasPermission(ViewExams)]
    [EndpointDescription("Retrieves a specific exam along with all its associated questions.")]
    [ProducesResponseType<ExamWithQuestionsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        var query = new GetExamByIdQuery(examId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates a new exam based on the provided details.</summary>
    /// <param name="command">The details of the exam to be created.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the newly created exam.</returns>
    [HttpPost("Create")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Creates a new exam based on the provided details.")]
    [ProducesResponseType<ExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateExamAsync(CreateExamCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates the details of an existing exam.</summary>
    /// <param name="command">The unique identifier of the exam to update and the new details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Details of the updated exam.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Updates the details of an existing exam.")]
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

    /// <summary>Publishes an exam, making it available to candidates.</summary>
    /// <param name="examId">The unique identifier of the exam to publish.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating the exam has been published.</returns>
    [HttpPost("Publish")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Publishes an exam, making it available to candidates.")]
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

    /// <summary>Invites a list of candidates to participate in a specific exam.</summary>
    /// <param name="command">The unique identifier of the exam and a list of candidate email addresses to invite.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating the invitations have been sent.</returns>
    [HttpPost("Invite/Candidates")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Invites a list of candidates to participate in a specific exam.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> InviteCandidatesAsync(InviteCandidatesCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a specific exam.</summary>
    /// <param name="examId">The unique identifier of the exam to delete.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response indicating the exam has been deleted.</returns>
    [HttpDelete("Delete/{examId:guid}")]
    [HasPermission(ManageExams)]
    [EndpointDescription("Deletes a specific exam.")]
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