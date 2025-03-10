using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Examinations.Commands;
using OPS.Application.Features.Examinations.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;

// [AuthorizeRoles(RoleType.Admin)]
[Route("api/Exam")]
public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("All")]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var query = new GetAllExamsQuery();
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpGet("{examId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId)
    {
        var query = new GetExamByIdQuery(examId);
        var response = await _mediator.Send(query);

        return ToResult(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateExamAsync(CreateExamCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync(UpdateExamCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAsync(DeleteExamCommand command)
    {
        var response = await _mediator.Send(command);

        return ToResult(response);
    }
}