using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Authentication.Commands;
using OPS.Application.Features.Authentication.Queries;

namespace OPS.Api.Controllers;

[AllowAnonymous]
[Route("api/Auth")]
public class AuthController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>User login with provided credentials.</summary>
    /// <param name="query">Login with username or email, and password.</param>
    /// <returns>Login token with account and profile details.</returns>
    [HttpPost("Login")]
    [EndpointDescription("User login with provided credentials.")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginQuery query)
    {
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    /// <summary>Registers a new user.</summary>
    /// <param name="command">Account details for a new user registration.</param>
    /// <returns>Account details with login token.</returns>
    [HttpPost("Register")]
    [EndpointDescription("Registers a new user.")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ConflictResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Recovers password for a user.</summary>
    /// <param name="command">Password recovery details.</param>
    /// <returns>Account details with login token.</returns>
    [HttpPost("PasswordRecovery")]
    [EndpointDescription("Recovers password for a user.")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PasswordRecoveryAsync(PasswordRecoveryCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Checks if a user is unique.</summary>
    /// <param name="query">Username and email (either or both) of an existing user.</param>
    /// <returns>Boolean indicating if the user is unique.</returns>
    [HttpPost("IsUserUnique")]
    [EndpointDescription("Checks if a user is unique.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsUserUniqueAsync(IsUserUniqueQuery query)
    {
        var response = await _mediator.Send(query);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isUnique = response.Value });
    }

    /// <summary>Sends an OTP to a specified user.</summary>
    /// <param name="command">Email to send an OTP.</param>
    /// <returns>Void</returns>
    [HttpPost("SendOtp")]
    [EndpointDescription("Sends an OTP to a specified user.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendOtpAsync(SendOtpCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    /// <summary>Validates a provided OTP.</summary>
    /// <param name="query">Otp verification details.</param>
    /// <returns>Boolean indicating if the OTP is valid.</returns>
    [HttpPost("IsValidOtp")]
    [EndpointDescription("Validates a provided OTP.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsValidOtpAsync(IsValidOtpQuery query)
    {
        var response = await _mediator.Send(query);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isValidOtp = response.Value });
    }
}