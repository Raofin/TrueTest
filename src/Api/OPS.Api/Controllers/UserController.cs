using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Api.Controllers;

public class UserController(
    IMediator mediator,
    IUserInfoProvider userInfoProvider) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    [HttpGet("UserInfo")]
    public IActionResult UserInfo()
    {
        return Ok(new
        {
            AccountId = _userInfoProvider.AccountId(),
            Username = _userInfoProvider.Username(),
            Email = _userInfoProvider.Email(),
            Roles = _userInfoProvider.Roles()
        });
    }
}