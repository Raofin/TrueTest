using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Auth.Commands;
using OPS.Application.Features.Auth.Queries;

namespace OPS.Api.Controllers;

public class AuthController(
    IMediator mediator,
    IValidator<LoginQuery> loginValidator,
    IValidator<RegisterCommand> registerValidator,
    IValidator<SendOtpCommand> sendOtpValidator) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<LoginQuery> _loginValidator = loginValidator;
    private readonly IValidator<RegisterCommand> _registerValidator = registerValidator;
    private readonly IValidator<SendOtpCommand> _sendOtpValidator = sendOtpValidator;

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginQuery query)
    {
        var validation = await _loginValidator.ValidateAsync(query);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        var result = await _mediator.Send(query);

        return result.IsError switch
        {
            false => Ok(result.Value),
            _ => result.Errors.Any(e => e.Type is ErrorType.NotFound or ErrorType.Unauthorized)
                ? Unauthorized("Invalid Credentials.")
                : Problem("An unexpected error occurred.")
        };
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command)
    {
        var validation = await _registerValidator.ValidateAsync(command);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.Conflict => Conflict("User with the provided email already exists."),
                ErrorType.Unauthorized => Unauthorized("Invalid or Expired Otp."),
                _ => Problem("An unexpected error occurred.")
            };
    }

    [HttpPost("IsUserUnique")]
    public async Task<IActionResult> IsUserUniqueAsync(IsUserUniqueQuery query)
    {
        var isUnique = await _mediator.Send(query);

        return isUnique
            ? Ok("Username or email is unique.")
            : Conflict("Username or email is already taken.");
    }

    [HttpPost("SendOtp")]
    public async Task<IActionResult> SendOtpAsync(SendOtpCommand command)
    {
        var validation = await _sendOtpValidator.ValidateAsync(command);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        await _mediator.Send(command);

        return Ok();
    }

    [HttpPost("IsValidOtp")]
    public async Task<IActionResult> IsValidOtpAsync(IsValidOtpQuery query)
    {
        var result = await _mediator.Send(query);

        return result
            ? Ok("Otp is valid.")
            : Unauthorized("Otp is invalid or expired.");
    }
}