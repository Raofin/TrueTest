using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Submissions.Commands;
using OPS.Application.Features.Submissions.Queries;

namespace OPS.Api.Controllers;

[Route("api/Submissions")]
public class SubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("Mcq/ByExam/{examId:guid}/{accountId:guid}")]
    public async Task<IActionResult> GetMcqSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetMcqQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPost("Mcq/Save")]
    public async Task<IActionResult> SaveMcqSubmissionAsync(SaveMcqSubmissionCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPost("Problem/Save")]
    public async Task<IActionResult> SaveProblemAsync(SaveProblemSubmissionCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpGet("Problem/{problemSubmissionId:guid}")]
    public async Task<IActionResult> GetProblemSubmissionAsync(Guid problemSubmissionId)
    {
        var query = new GetProblemSubmissionQuery(problemSubmissionId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpGet("Problem/ByExam/{examId:guid}/{accountId:guid}")]
    public async Task<IActionResult> GetProblemSubmissionsByExamAsync(Guid examId, Guid accountId)
    {
        var query = new GetProblemQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }
    
    [HttpGet("Written/ByExam/{examId:guid}/{accountId:guid}")]
    public async Task<IActionResult> GetWrittenSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var response = await _mediator.Send(query);
    
        return ToResult(response);
    }
    
    [HttpPost("Written/Save")]
    public async Task<IActionResult> SaveWrittenSubmissionAsync(SaveWrittenSubmissionCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
}