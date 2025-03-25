using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ProblemResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;

namespace OPS.Api.Controllers;

[Route("api/Account")]
// [AuthorizeRoles(RoleType.Admin)]
[ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves a list of all available accounts.</summary>
    /// <returns>A list of account objects.</returns>
    [HttpGet("All")]
    [ProducesResponseType(typeof(List<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAccounts()
    {
        var query = new GetAllAccountsQuery();
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    /// <summary>Changes the active status of an account.</summary>
    /// <param name="command">Contains the account ID to change status.</param>
    /// <returns>The updated account object.</returns>
    [HttpPatch("ChangeActiveStatus")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeActiveStatus(ChangeActiveStatusCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Updates the details of an account.</summary>
    /// <param name="command">Contains the account ID and updated info.</param>
    /// <returns>The updated account object.</returns>
    [HttpPut("Update")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ConflictResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Makes an account an admin.</summary>
    /// <param name="command">Contains the account ID to make admin.</param>
    /// <returns>The updated account object.</returns>
    [HttpPost("MakeAdmin")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeAdmin(MakeAdminCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Sends an admin invite to an email address.</summary>
    /// <param name="command">Contains the email address to send admin invite.</param>
    /// <returns>A success response if the invite was sent.</returns>
    [HttpPost("SendAdminInvite")]
    [ProducesResponseType(typeof(NoContent), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendAdminInvite(SendAdminInviteCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
}