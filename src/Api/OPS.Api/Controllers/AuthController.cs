using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.Authentication.Commands;
using OPS.Application.Features.Authentication.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("Auth")]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class AuthController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>User login with provided credentials.</summary>
    /// <param name="query">Login with username or email, and password.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Login token with account and profile details.</returns>
    [HttpPost("Login")]
    [AllowAnonymous]
    [EndpointDescription("User login with provided credentials.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> LoginAsync(LoginQuery query, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Registers a new user.</summary>
    /// <param name="command">Account details for a new user registration.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Account details with login token.</returns>
    [HttpPost("Register")]
    [AllowAnonymous]
    [EndpointDescription("Registers a new user.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Recovers password for a user.</summary>
    /// <param name="command">Password recovery details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Account details with login token.</returns>
    [HttpPost("PasswordRecovery")]
    [AllowAnonymous]
    [EndpointDescription("Recovers password for a user.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> PasswordRecoveryAsync(PasswordRecoveryCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Checks if a user is unique.</summary>
    /// <param name="query">Username and email (either or both) of an existing user.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Boolean indicating if the user is unique.</returns>
    [HttpPost("IsUserUnique")]
    [AllowAnonymous]
    [EndpointDescription("Checks if a user is unique.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> IsUserUniqueAsync(IsUserUniqueQuery query,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(query, cancellationToken);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isUnique = response.Value });
    }

    /// <summary>Sends an OTP to a user.</summary>
    /// <param name="command">Email to send an OTP.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Void</returns>
    [HttpPost("SendOtp")]
    [AllowAnonymous]
    [EndpointDescription("Sends an OTP to a user.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> SendOtpAsync(SendOtpCommand command, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Validates a provided OTP.</summary>
    /// <param name="query">Otp verification details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Boolean indicating if the OTP is valid.</returns>
    [HttpPost("IsValidOtp")]
    [AllowAnonymous]
    [EndpointDescription("Validates a provided OTP.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> IsValidOtpAsync(IsValidOtpQuery query,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(query, cancellationToken);

        return response.IsError
            ? Problem(response.Errors)
            : Ok(new { isValidOtp = response.Value });
    }
}