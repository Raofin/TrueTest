using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Application.Features.Questions.ProblemSolving.Queries;

namespace OPS.Api.Controllers.Questions;

public class ProblemSolvingController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProblemSolvingCommand command)
    {
        var problemSolving = await _mediator.Send(command);

        return ToResult(problemSolving);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid questionId)
    {
        var query = new GetProblemSolvingByIdQuery(questionId);
        var problemSolving = await _mediator.Send(query);

        return ToResult(problemSolving);
    }
    
    [HttpGet("GetByExamId/{examId:guid}")]
    public async Task<IActionResult> GetAllByExamIdAsync(Guid examId)
    {
        var query = new GetAllProblemSolvingByExamIdQuery(examId);
        var problemSolving = await _mediator.Send(query);

        return ToResult(problemSolving);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProblemSolvingCommand command)
    {
        var problemSolving = await _mediator.Send(command);

        return ToResult(problemSolving);
    }
    
    [HttpDelete("TestCase/{testCaseId:guid}")]
    public async Task<IActionResult> DeleteTestCaseAsync(Guid testCaseId)
    {
        var query = new DeleteTestCaseCommand(testCaseId);
        var deleted = await _mediator.Send(query);

        return ToResult(deleted);
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid questionId)
    {
        var query = new DeleteProblemSolvingCommand(questionId);
        var deleted = await _mediator.Send(query);

        return ToResult(deleted);
    }
}