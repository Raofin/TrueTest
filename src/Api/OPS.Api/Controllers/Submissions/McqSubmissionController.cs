using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Submissions.McqSubmissions.Commands;
using OPS.Application.Features.Submissions.McqSubmissions.Queries;

namespace OPS.Api.Controllers.Submissions;

public class McqSubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("GetByExamId/{examId:guid}/{accountId:guid}")]
    public async Task<IActionResult> GetMcqSubmissionsAsync(Guid examId, Guid accountId)
    {
        var query = new GetMcqQuesWithSubmissionQuery(examId, accountId);
        var result = await _mediator.Send(query);

        return ToResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(SaveMcqSubmissionCommand command)
    {
        var createdMcqSubmission = await _mediator.Send(command);

        return ToResult(createdMcqSubmission);
    }
}