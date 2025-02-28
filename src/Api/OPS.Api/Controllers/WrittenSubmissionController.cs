using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.WrittenSubmissions.Commands;
using OPS.Application.Features.WrittenSubmissions.Queries;

namespace OPS.Api.Controllers;

public class WrittenSubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("All")]
    public async Task<IActionResult> GetAllWrittenSubmissionsAsync()
    {
        var result = await _mediator.Send(new GetAllWrittenSubmissionQuery());

        return ToResult(result);
    }

    [HttpGet("writtenSubmissionId/{writtenSubmissionId:guid}")]
    public async Task<IActionResult> GetWrittenSubmissionByIdAsync(Guid writtenSubmissionId)
    {
        var result = await _mediator.Send(new GetWrittenSubmissionByIdQuery(writtenSubmissionId));

        return ToResult(result);
    }

    [HttpGet("questionId/{questionId:guid}")]
    public async Task<IActionResult> GetWrittenSubmissionByQuestionIdIdAsync(Guid questionId)
    {
        var result = await _mediator.Send(new GetWrittenSubmissionByQuestionIdQuery(questionId));

        return ToResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateWrittenSubmissionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateWrittenSubmissionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
}