using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Application.Features.Questions.Written.Queries;

namespace OPS.Api.Controllers;

[Route("api/Questions/Written")]
public class QuestionWrittenController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Create")]
    public async Task<IActionResult> CreateWrittenAsync(CreateWrittenCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetWrittenByIdAsync(Guid questionId)
    {
        var query = new GetWrittenByIdQuery(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpGet("ByExam/{examId:guid}")]
    public async Task<IActionResult> GetWrittenByExamAsync(Guid examId)
    {
        var query = new GetAllWrittenByExamIdQuery(examId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateWrittenAsync(UpdateWrittenCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpDelete("Delete/{questionId:guid}")]
    public async Task<IActionResult> DeleteWrittenAsync(Guid questionId)
    {
        var query = new DeleteWrittenCommand(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }
}