using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.ProblemQuestions.Commands;
using OPS.Application.Features.ProblemQuestions.Queries;

namespace OPS.Api.Controllers;

public class ProblemQuestionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet("Exam/{ExamId:guid}")]
    public async Task<IActionResult> GetAllProblemQuestionByExamIdAsync(Guid ExamId)
    {
        var result = await _mediator.Send(new GetProblemQuestionsByExamIdQuery(ExamId));

        return ToResult(result);
    }

    [HttpGet("ProblemQuestion/{ProblemQuestionId:guid}")]
    public async Task<IActionResult> GetProblemQuestionByIdAsync(Guid ProblemQuestionId)
    {
        var result = await _mediator.Send(new GetProblemQuestionByIdQuery(ProblemQuestionId));

        return ToResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProblemQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProblemQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }

    [HttpDelete("{ProblemQuestionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid ProblemQuestionId)
    {
        var result = await _mediator.Send(new DeleteProblemQuestionCommand(ProblemQuestionId));

        return ToResult(result);
    }
}