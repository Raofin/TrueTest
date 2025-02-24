using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Examinations.Commands;
using OPS.Application.Features.Examinations.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;

public class ExamController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var exams = await _mediator.Send(new GetAllExamsQuery());

        return ToResult(exams);
    }

    [HttpGet("{examId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(GetExamByIdQuery query)
    {
        var exam = await _mediator.Send(query);

        return ToResult(exam);
    }

    [HttpGet("UpcomingExams")]
    public async Task<IActionResult> GetUpcomingExamsAsync()
    {
        var upcomingExams = await _mediator.Send(new GetUpcomingExams());

        return ToResult(upcomingExams);
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
    
    [HttpGet("UpcomingExams/{accountId:guid}")]
    public async Task<IActionResult> GetUpcomingExamsByAccountIdAsync(Guid accountId)
    {
        var result = await _mediator.Send(new GetUpcomingExamsByAccountIdQuery(accountId));

        return ToResult(result);   
    }

    [HttpGet("PreviousExams/{accountId:guid}")]
    public async Task<IActionResult> GetPreviousExamsByAccountIdAsync(Guid accountId)
    {
        var result = await _mediator.Send(new GetPreviousExamsByAccountIdQuery(accountId));

        return ToResult(result);    
    }
}