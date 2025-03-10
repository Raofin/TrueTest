using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Application.Features.Questions.Mcq.Queries;

namespace OPS.Api.Controllers;

[Route("Questions/Mcq")]
public class QuestionMcqController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Create")]
    public async Task<IActionResult> CreateMcqAsync(CreateMcqCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetMcqByIdAsync(Guid questionId)
    {
        var query = new GetMcqQuestionByIdQuery(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpGet("ByExam/{examId:guid}")]
    public async Task<IActionResult> GetMcqByExamAsync(Guid examId)
    {
        var query = new GetAllMcqByExamIdQuery(examId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync(UpdateMcqCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpDelete("Delete/{questionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid questionId)
    {
        var query = new DeleteMcqCommand(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }
}