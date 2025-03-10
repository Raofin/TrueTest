using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Submissions.Written.Commands;
using OPS.Application.Features.Submissions.Written.Queries;

namespace OPS.Api.Controllers;

public class WrittenSubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet("GetByExamId/{examId:guid}/{accountId:guid}")]
    public async Task<IActionResult> GetAllWrittenSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetWrittenQuesWithSubmissionQuery(examId, accountId);
        var result = await _mediator.Send(query);

        return ToResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveWrittenSubmissionAsync(SaveWrittenSubmissionCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
}