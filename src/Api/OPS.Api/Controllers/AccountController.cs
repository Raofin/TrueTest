using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("Account")]
// [AuthorizeRoles(RoleType.Admin)]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves accounts with details.</summary>
    /// <param name="pageIndex">Page number.</param>
    /// <param name="pageSize">Accounts per page.</param>
    /// <param name="searchTerm">Optional search filter.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Paginated account list.</returns>
    [HttpGet]
    [EndpointDescription("Retrieves accounts with details.")]
    [ProducesResponseType<PaginatedAccountResponse>(Status200OK)]
    public async Task<IActionResult> GetAllAccountsAsync(int pageIndex = 1, int pageSize = 10,
        string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = new GetAllAccountsQuery(pageIndex, pageSize, searchTerm);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Changes active status of an account.</summary>
    /// <param name="command">Account ID to change active status.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated account object.</returns>
    [HttpPatch("ChangeActiveStatus")]
    [EndpointDescription("Changes active status of an account.")]
    [ProducesResponseType<AccountResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ChangeActiveStatusAsync(ChangeActiveStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates details of an account.</summary>
    /// <param name="command">Account ID and updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated account object.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates details of an account.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAccountAsync(UpdateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Upgrades a list of accounts to admin.</summary>
    /// <param name="command">Account ID List.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response if the accounts are upgraded.</returns>
    [HttpPost("MakeAdmin")]
    [EndpointDescription("Upgrades a list of accounts to admin")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> MakeAdminAsync(MakeAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Sends admin invite to a list of email addresses.</summary>
    /// <param name="command">Email address list.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Success response.</returns>
    [HttpPost("SendAdminInvite")]
    [EndpointDescription("Sends admin invite to a list of email addresses.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> SendAdminInviteAsync(SendAdminInviteCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes an account.</summary>
    /// <param name="accountId">Account ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A success response if the account was deleted.</returns>
    [HttpDelete("Delete/{accountId:guid}")]
    [EndpointDescription("Deletes an account.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteAccountCommand(accountId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }
}