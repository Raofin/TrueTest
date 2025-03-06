using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.WrittenQuestions.Queries;

namespace OPS.Api.Controllers;

public class WrittenQuestionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet("Exam/{ExamId:guid}")]
    public async Task<IActionResult> GetAllWrittenQuestionByExamIdAsync(Guid ExamId)
    {
        var result = await _mediator.Send(new GetWrittenQuestionsByExamIdQuery(ExamId));

        return ToResult(result);
    }
}