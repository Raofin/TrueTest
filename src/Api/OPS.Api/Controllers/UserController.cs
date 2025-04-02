using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Examinations.Queries;
using OPS.Application.Features.User.Commands;
using OPS.Application.Features.User.Queries;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Api.Controllers;

[Route("api/User")]
public class UserController(IMediator mediator, IUserInfoProvider userInfoProvider) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    [HttpGet("Info")]
    public IActionResult GetInfo()
    {
        return !_userInfoProvider.IsAuthenticated()
            ? Unauthorized("User is not authenticated.")
            : Ok(new
            {
                AccountId = _userInfoProvider.AccountId(),
                Username = _userInfoProvider.Username(),
                Email = _userInfoProvider.Email(),
                Roles = _userInfoProvider.Roles()
            });
    }

    [HttpPatch("AccountSettings")]
    public async Task<IActionResult> UpdateAccountSettingsAsync(UpdateAccountSettingsCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("Details")]
    public async Task<IActionResult> GetDetailsAsync()
    {
        var query = new GetUserDetailsQuery();
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPost("SaveProfile")]
    public async Task<IActionResult> CreateAsync(CreateOrUpdateProfileCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpDelete("ProfileLink")]
    public async Task<IActionResult> DeleteProfileLinkAsync(DeleteProfileLinkCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("Exams")]
    public async Task<IActionResult> GetExamsAsync(GetExamsByUserQuery query)
    {
        var response = await _mediator.Send(query);

        return ToResult(response);
    }
}