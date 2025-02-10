using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Application.Features.Auth.Commands;
using OPS.Application.Features.Auth.Queries;

namespace OPS.Api.Controllers;


public class AccountController(
    IMediator mediator,
    IValidator<CreateAccountCommand> _createAccountValidator, 
    IValidator<UpdateProfileCommand> _updateAccountValidator) : ApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllAccountsAsync()
    {
        var query = new GetAllAccountsQuery();
        
        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }

    [HttpGet("{AccountId:long}")]
    public async Task<IActionResult> GetAccountByIdAsync(long AccountId)
    {
        var query = new GetAccountByIdQuery(AccountId);
        
        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Account was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAccountCommand command)
    {
        var validation = await _createAccountValidator.ValidateAsync(command);
        
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }
        
        var result = await _mediator.Send(command);
        
        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProfileCommand command)
    {
        var validation = await _updateAccountValidator.ValidateAsync(command);
        
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }
        
        var result = await _mediator.Send(command);
        
        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(long AccountId)
    {
        var command = new DeleteAccountCommand(AccountId);
        
        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok("Account was deleted.")
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Account was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }
}
