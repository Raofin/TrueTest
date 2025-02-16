using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.Exams.Commands;
using OPS.Application.Features.Exams.Queries;

namespace OPS.Api.Controllers;

public class ExamController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync()
    {
        var query = new GetAllExamsQuery();

        var exams = await _mediator.Send(query);

        return ToResult(exams);
    }

    [HttpGet("{examId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid examId)
    {
        var query = new GetExamByIdQuery(examId);

        var exam = await _mediator.Send(query);

        return ToResult(exam);
    }

    [HttpGet("UpcomingExams")]
    public async Task<IActionResult> GetUpcomingExamsAsync()
    {
        var query = new GetUpcomingExams();

        var upcomingExams = await _mediator.Send(query);

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
    public async Task<IActionResult> DeleteAsync(Guid examId)
    {
        var command = new DeleteExamCommand(examId);

        var deleteResult = await _mediator.Send(command);

        return ToResult(deleteResult);
    }
}