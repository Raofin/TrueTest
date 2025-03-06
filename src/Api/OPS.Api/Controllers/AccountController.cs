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

    [HttpPatch("ToggleActiveStatus")]
    public async Task<IActionResult> ToggleActiveStatus(ChangeActiveStatusCommand command)
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
}