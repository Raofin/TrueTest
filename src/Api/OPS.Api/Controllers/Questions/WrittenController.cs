using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Application.Features.Questions.Written.Queries;

namespace OPS.Api.Controllers.Questions;

public class WrittenController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateWrittenCommand command)
    {
        var question = await _mediator.Send(command);

        return ToResult(question);
    }
    
    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetWrittenQuestionByIdAsync(Guid questionId)
    {
        var query = new GetWrittenByIdQuery(questionId);
        var question = await _mediator.Send(query);

        return ToResult(question);
    }
    
    [HttpGet("GetByExamId/{examId:guid}")]
    public async Task<IActionResult> GetAllByExamIdAsync(Guid examId)
    {
        var query = new GetAllWrittenByExamIdQuery(examId);
        var questions = await _mediator.Send(query);

        return ToResult(questions);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateWrittenCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid questionId)
    {
        var query = new DeleteWrittenCommand(questionId);
        var deleted = await _mediator.Send(query);

        return ToResult(deleted);
    }
}