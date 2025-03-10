using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Application.Features.Questions.ProblemSolving.Queries;

namespace OPS.Api.Controllers;

[Route("Questions/Problem")]
public class QuestionProblemController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProblemSolvingAsync(CreateProblemSolvingCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetProblemSolvingByIdAsync(Guid questionId)
    {
        var query = new GetProblemSolvingByIdQuery(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpGet("ByExam/{examId:guid}")]
    public async Task<IActionResult> GetProblemSolvingByExamAsync(Guid examId)
    {
        var query = new GetAllProblemSolvingByExamIdQuery(examId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProblemSolvingAsync(UpdateProblemSolvingCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpDelete("TestCase/{testCaseId:guid}")]
    public async Task<IActionResult> DeleteTestCaseAsync(Guid testCaseId)
    {
        var query = new DeleteTestCaseCommand(testCaseId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpDelete("Delete/{questionId:guid}")]
    public async Task<IActionResult> DeleteProblemSolvingAsync(Guid questionId)
    {
        var query = new DeleteProblemSolvingCommand(questionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }
}