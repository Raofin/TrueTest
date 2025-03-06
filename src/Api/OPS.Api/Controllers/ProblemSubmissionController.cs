using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.ProblemSubmissions.Commands;
using OPS.Application.Features.ProblemSubmissions.Queries;

namespace OPS.Api.Controllers;

public class ProblemSubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet("{ProblemSubmissionId:guid}")]
    public async Task<IActionResult> GetProblemSubmissionByIdAsync(Guid ProblemSubmissionId)
    {
        var result = await _mediator.Send(new GetProblemSubmissionByIdQuery(ProblemSubmissionId));

        return ToResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProblemSubmissionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProblemSubmissionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
}