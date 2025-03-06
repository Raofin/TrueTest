using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.McqSubmissions.Commands;
using OPS.Application.Features.McqSubmissions.Queries;

namespace OPS.Api.Controllers;

public class McqSubmissionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllMcqSubmission()
    {
        var mcqSubmission = await _mediator.Send(new GetAllMcqSubmissionQuery());

        return ToResult(mcqSubmission);
    }

    [HttpGet("{mcqSubmissionId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid mcqSubmissionId)
    {
        var result = await _mediator.Send(new GetProblemSubmissionByIdQuery(mcqSubmissionId));

        return ToResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateMcqSubmissionCommand command)
    {
        var createdMcqSubmission = await _mediator.Send(command);

        return ToResult(createdMcqSubmission);
    }

    // [HttpPut]
    // public async Task<IActionResult> UpdateAccount(UpdateMcqQuestionCommand command)
    // {
    //     var mcqSubmission = await _mediator.Send(command);
    //
    //     return ToResult(mcqSubmission);
    // }
}