using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;

// [AuthorizeRoles(RoleType.Admin)]
[Route("Account")]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("AllAccounts")]
    public async Task<IActionResult> GetAllAccounts()
    {
        var query = new GetAllAccountsQuery();
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPatch("ChangeActiveStatus")]
    public async Task<IActionResult> ChangeActiveStatus(ChangeActiveStatusCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
    
    [HttpPost("MakeAdmin")]
    public async Task<IActionResult> MakeAdmin(MakeAdminCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
    
    [HttpPost("SendAdminInvite")]
    public async Task<IActionResult> SendAdminInvite(SendAdminInviteCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
}