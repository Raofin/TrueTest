using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;

namespace OPS.Api.Controllers;

public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _mediator.Send(new GetAllAccountsQuery());

        return ToResult(accounts);
    }

    [HttpPatch("ChangeActiveStatus")]
    public async Task<IActionResult> ChangeActiveStatus(ChangeActiveStatusCommand command)
    {
        var status = await _mediator.Send(command);

        return ToResult(status);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand command)
    {
        var updatedAccount = await _mediator.Send(command);

        return ToResult(updatedAccount);
    }
    
    [HttpPost("MakeAdmin")]
    public async Task<IActionResult> MakeAdmin(MakeAdminCommand command)
    {
        var account = await _mediator.Send(command);

        return ToResult(account);
    }
    
    [HttpPost("SendAdminInvite")]
    public async Task<IActionResult> SendAdminInvite(SendAdminInviteCommand command)
    {
        var invite = await _mediator.Send(command);

        return ToResult(invite);
    }
}