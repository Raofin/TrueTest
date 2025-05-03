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

/// <summary>
/// API endpoints for user authentication and registration.
/// </summary>
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
    [EndpointDescription("Authenticates a user and returns a login token with account and profile details.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status403Forbidden)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> LoginAsync(LoginQuery query, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Registers a new user.</summary>
    /// <param name="command">Account details for a new user registration.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Account details with login token upon successful registration.</returns>
    [HttpPost("Register")]
    [AllowAnonymous]
    [EndpointDescription("Registers a new user and returns account details with a login token.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status403Forbidden)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Recovers password for a user.</summary>
    /// <param name="command">Details required for initiating password recovery.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Account details with a new login token upon successful password recovery.</returns>
    [HttpPost("PasswordRecovery")]
    [AllowAnonymous]
    [EndpointDescription("Initiates the password recovery process for a user and returns account details with a new login token.")]
    [ProducesResponseType<AuthenticationResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<UnauthorizedResponse>(Status403Forbidden)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> PasswordRecoveryAsync(PasswordRecoveryCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Checks if a user with the provided username or email is unique.</summary>
    /// <param name="query">Username and/or email to check for uniqueness.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Boolean indicating if the provided username or email is unique.</returns>
    [HttpPost("IsUserUnique")]
    [AllowAnonymous]
    [EndpointDescription("Checks if a user with the provided username or email is unique.")]
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

    /// <summary>Sends a One-Time Password (OTP) to the provided email address.</summary>
    /// <param name="command">Email address to which the OTP will be sent.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>No content upon successful OTP submission.</returns>
    [HttpPost("SendOtp")]
    [AllowAnonymous]
    [EndpointDescription("Sends a One-Time Password (OTP) to the provided email address.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> SendOtpAsync(SendOtpCommand command, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Validates a provided One-Time Password (OTP).</summary>
    /// <param name="query">OTP and the associated email for verification.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Boolean indicating if the provided OTP is valid for the given email.</returns>
    [HttpPost("IsValidOtp")]
    [AllowAnonymous]
    [EndpointDescription("Validates a provided One-Time Password (OTP) for a given email.")]
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