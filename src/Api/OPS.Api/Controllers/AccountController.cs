using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Features.Accounts.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;

[AuthorizeRoles(RoleType.Admin)]
public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _mediator.Send(new GetAllAccountsQuery());

        return ToResult(accounts);
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAllAccountsByThisMonth()
    // {
    //     var accounts = await _mediator.Send(new GetAllAccountsByThisMonthQuery());
    //
    //     return ToResult(accounts);
    // }

    [HttpPatch("ToggleActiveStatus")]
    public async Task<IActionResult> ToggleActiveStatus(ToggleActiveStatusCommand command)
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