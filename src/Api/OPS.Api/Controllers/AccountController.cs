using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;
using OPS.Domain.Enums;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OPS.Domain.Constants.Permissions;

namespace OPS.Api.Controllers;

[Route("Account")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves accounts with details.</summary>
    /// <param name="pageIndex">Page number.</param>
    /// <param name="pageSize">Accounts per page.</param>
    /// <param name="searchTerm">Optional search filter.</param>
    /// <param name="role">Optional account role</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Paginated account list.</returns>
    [HttpGet]
    [HasPermission(ManageAccounts)]
    [EndpointDescription("Retrieves accounts with details.")]
    [ProducesResponseType<PaginatedAccountResponse>(Status200OK)]
    public async Task<IActionResult> GetAllAccountsAsync(int pageIndex = 1, int pageSize = 10,
        string? searchTerm = null, RoleType? role = null, CancellationToken cancellationToken = default)
    {
        var query = new GetAllAccountsQuery(pageIndex, pageSize, searchTerm, role);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Changes active status of an account.</summary>
    /// <param name="accountId">Account ID to change active status.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated account object.</returns>
    [HttpPatch("ChangeActiveStatus/{accountId:guid}")]
    [HasPermission(ManageAccounts)]
    [EndpointDescription("Changes active status of an account.")]
    [ProducesResponseType<AccountResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ChangeActiveStatusAsync(Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var command = new ChangeActiveStatusCommand(accountId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates details of an account.</summary>
    /// <param name="command">Account ID and updated details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated account object.</returns>
    [HttpPatch("Update")]
    [HasPermission(ManageAccounts)]
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
    [HttpPatch("MakeAdmin")]
    [HasPermission(ManageAccounts)]
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
    [HasPermission(ManageAccounts)]
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
    [HasPermission(ManageAccounts)]
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