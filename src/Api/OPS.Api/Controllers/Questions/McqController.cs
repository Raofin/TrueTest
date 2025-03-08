using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Application.Features.Questions.Mcq.Queries;

namespace OPS.Api.Controllers.Questions;

public class McqController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateMcqCommand command)
    {
        var question = await _mediator.Send(command);

        return ToResult(question);
    }
    
    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetMcqQuestionByIdAsync(Guid questionId)
    {
        var query = new GetMcqQuestionByIdQuery(questionId);
        var question = await _mediator.Send(query);

        return ToResult(question);
    }
    
    [HttpGet("GetByExamId/{examId:guid}")]
    public async Task<IActionResult> GetAllByExamIdAsync(Guid examId)
    {
        var query = new GetAllMcqByExamIdQuery(examId);
        var questions = await _mediator.Send(query);

        return ToResult(questions);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateMcqCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid questionId)
    {
        var query = new DeleteMcqCommand(questionId);
        var deleted = await _mediator.Send(query);

        return ToResult(deleted);
    }
}