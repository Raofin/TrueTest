using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.WrittenSubmissions.Commands;
using OPS.Application.Features.WrittenSubmissions.Queries;

namespace OPS.Api.Controllers;

public class WrittenSubmissionController(
    IMediator mediator,
    IValidator<CreateWrittenSubmissionCommand> createWrittenSubmissionValidator,
    IValidator<UpdateWrittenSubmissionCommand> updateWrittenSubmissionValidator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<CreateWrittenSubmissionCommand> _createWrittenSubmissionValidator = createWrittenSubmissionValidator;
    private readonly IValidator<UpdateWrittenSubmissionCommand> _updateWrittenSubmissionValidator = updateWrittenSubmissionValidator;

    [HttpGet("All")]
    public async Task<IActionResult> GetAllWrittenSubmissionsAsync()
    {
        var query = new GetAllWrittenSubmissionQuery();

        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }

    [HttpGet("writtenSubmissionId/{writtenSubmissionId:guid}")]
    public async Task<IActionResult> GetWrittenSubmissionByIdAsync(Guid writtenSubmissionId)
    {
        var query = new GetWrittenSubmissionByIdQuery(writtenSubmissionId);

        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("WrittenSubmission was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }

    [HttpGet("questionId/{questionId:guid}")]
    public async Task<IActionResult> GetWrittenSubmissionByQuestionIdIdAsync(Guid questionId)
    {
        var query = new GetWrittenSubmissionByQuestionIdQuery(questionId);

        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("WrittenSubmission was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateWrittenSubmissionCommand command)
    {
        var validation = await _createWrittenSubmissionValidator.ValidateAsync(command);

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
    public async Task<IActionResult> UpdateAsync(UpdateWrittenSubmissionCommand command)
    {
        var validation = await _updateWrittenSubmissionValidator.ValidateAsync(command);

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
}