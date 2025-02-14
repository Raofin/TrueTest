using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.User.Commands;
using OPS.Application.Features.User.Queries;

namespace OPS.Api.Controllers;

public class UserController(
    IMediator mediator,
    IValidator<LoginQuery> loginValidator,
    IValidator<RegisterCommand> registerValidator) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<LoginQuery> _loginValidator = loginValidator;
    private readonly IValidator<RegisterCommand> _registerValidator = registerValidator;

    [HttpPost("login")]
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

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterCommand command)
    {
        var validation = await _registerValidator.ValidateAsync(command);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        var result = await _mediator.Send(command);

        return result.IsError switch
        {
            false => Ok(result.Value),
            _ => result.Errors.Any(e => e.Type is ErrorType.Conflict)
                ? Conflict("User with the provided email already exists.")
                : Problem("An unexpected error occurred.")
        };
    }
}