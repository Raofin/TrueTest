using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Account")]
// [AuthorizeRoles(RoleType.Admin)]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all accounts with details.</summary>
    /// <returns>A list of account objects.</returns>
    [HttpGet("All")]
    [EndpointDescription("Retrieves all accounts with details.")]
    [ProducesResponseType<List<AccountWithDetailsResponse>>(Status200OK)]
    public async Task<IActionResult> GetAllAccounts()
    {
        var query = new GetAllAccountsQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Changes active status of an account.</summary>
    /// <param name="command">Account ID to change active status.</param>
    /// <returns>The updated account object.</returns>
    [HttpPatch("ChangeActiveStatus")]
    [EndpointDescription("Changes active status of an account.")]
    [ProducesResponseType<AccountResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ChangeActiveStatus(ChangeActiveStatusCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Updates details of an account.</summary>
    /// <param name="command">Account ID and updated details.</param>
    /// <returns>The updated account object.</returns>
    [HttpPut("Update")]
    [EndpointDescription("Updates details of an account.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Upgrades an account to admin.</summary>
    /// <param name="command">Account ID to make admin.</param>
    /// <returns>The updated account object.</returns>
    [HttpPost("MakeAdmin")]
    [EndpointDescription("Upgrades an account to admin.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> MakeAdmin(MakeAdminCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Sends admin invite to an email address.</summary>
    /// <param name="command">Email address to send admin invite.</param>
    /// <returns>A success response if the invite was sent.</returns>
    [HttpPost("SendAdminInvite")]
    [EndpointDescription("Sends admin invite to an email address.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> SendAdminInvite(SendAdminInviteCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes an account.</summary>
    /// <param name="accountId">Account ID.</param>
    /// <returns>A success response if the account was deleted.</returns>
    [HttpDelete("Delete/{accountId:guid}")]
    [EndpointDescription("Deletes an account.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        var command = new DeleteAccountCommand(accountId);
        var response = await _mediator.Send(command);
        return ToResult(response);
    }
}