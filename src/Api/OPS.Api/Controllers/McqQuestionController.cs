using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.McqQuestions.Commands;
using OPS.Application.Features.McqQuestions.Queries;

namespace OPS.Api.Controllers;

public class McqQuestionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet("mcqQuestionsByExamId/{examId:guid}")]
    public async Task<IActionResult> GetAllMcqQuestionByExamIdAsync(Guid examId)
    {
        var result = await _mediator.Send(new GetMcqQuestionsByExamIdQuery(examId));

        return ToResult(result);
    }
    
    [HttpGet("mcqQuestion/{mcqQuestionId:guid}")]
    public async Task<IActionResult> GetMcqQuestionByIdAsync(Guid mcqQuestionId)
    {
        var result = await _mediator.Send(new GetMcqQuestionByIdQuery(mcqQuestionId));

        return ToResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateMcqQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateMcqQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpDelete("{mcqQuestionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid mcqQuestionId)
    {
        var result = await _mediator.Send(new DeleteMcqQuestionCommand(mcqQuestionId));

        return ToResult(result);
    }
}