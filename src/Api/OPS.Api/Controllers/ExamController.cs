using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Exams.Commands;
using OPS.Application.Features.Exams.Queries;

namespace OPS.Api.Controllers;

public class ExamController(
    IMediator mediator,
    IValidator<CreateExamCommand> createExamValidator,
    IValidator<UpdateExamCommand> updateExamValidator) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<CreateExamCommand> _createExamValidator = createExamValidator;
    private readonly IValidator<UpdateExamCommand> _updateExamValidator = updateExamValidator;

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var query = new GetAllExamsQuery();

        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }

    [HttpGet("{examId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId)
    {
        var query = new GetExamByIdQuery(examId);

        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Exam was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }

    [HttpGet("UpcomingExams")]
    public async Task<IActionResult> GetUpcomingExamsAsync()
    {
        var query = new GetUpcomingExams();

        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateExamCommand command)
    {
        var validation = await _createExamValidator.ValidateAsync(command);

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
    public async Task<IActionResult> UpdateAsync(UpdateExamCommand command)
    {
        var validation = await _updateExamValidator.ValidateAsync(command);

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
    public async Task<IActionResult> DeleteAsync(Guid examId)
    {
        var command = new DeleteExamCommand(examId);

        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok("Exam was deleted.")
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Exam was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }
}