using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Examinations.Commands;
using OPS.Application.Features.Examinations.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;

// [AuthorizeRoles(RoleType.Admin)]
public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var exams = await _mediator.Send(new GetAllExamsQuery());

        return ToResult(exams);
    }

    [HttpGet("{examId}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId)
    {
        var query = new GetExamByIdQuery(examId);
        var exam = await _mediator.Send(query);
        
        return ToResult(exam);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateExamCommand command)
    {
        var createdExam = await _mediator.Send(command);

        return ToResult(createdExam);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateExamCommand command)
    {
        var updatedExam = await _mediator.Send(command);

        return ToResult(updatedExam);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(DeleteExamCommand command)
    {
        var deleteResult = await _mediator.Send(command);

        return ToResult(deleteResult);
    }
}