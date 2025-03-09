using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.ProblemSubmissions.Commands;
using OPS.Application.Features.ProblemSubmissions.Queries;

namespace OPS.Api.Controllers;

public class ProblemSubmitController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProblemSubmitCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpGet("{problemSubmissionId:guid}")]
    public async Task<IActionResult> GetProblemSubmitByIdAsync(Guid problemSubmissionId)
    {
        var query = new GetProblemSubmissionByIdQuery(problemSubmissionId);
        var result = await _mediator.Send(query);

        return ToResult(result);
    }
    
    /*[HttpGet("GetByExamId/{examId:guid}")]
    public async Task<IActionResult> GetAllProblemSubmitsByExamIdAsync(Guid examId)
    {
        var query = new GetAllProblemSubmitsByExamIdQuery(examId);
        var result = await _mediator.Send(query);

        return ToResult(result);
    }*/
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProblemSubmitCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
}