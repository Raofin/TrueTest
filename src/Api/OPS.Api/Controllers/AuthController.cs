using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Authentication.Commands;
using OPS.Application.Features.Authentication.Queries;

namespace OPS.Api.Controllers;

[AllowAnonymous]
[Route("api/Auth")]
public class AuthController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginQuery query)
    {
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPost("PasswordRecovery")]
    public async Task<IActionResult> PasswordRecoveryAsync(PasswordRecoveryCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPost("IsUserUnique")]
    public async Task<IActionResult> IsUserUniqueAsync(IsUserUniqueQuery query)
    {
        var response = await _mediator.Send(query);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isUnique = response.Value });
    }

    [HttpPost("SendOtp")]
    public async Task<IActionResult> SendOtpAsync(SendOtpCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPost("IsValidOtp")]
    public async Task<IActionResult> IsValidOtpAsync(IsValidOtpQuery query)
    {
        var response = await _mediator.Send(query);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isValidOtp = response.Value });
    }
}