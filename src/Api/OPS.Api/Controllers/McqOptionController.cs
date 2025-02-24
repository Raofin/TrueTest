using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.ExamQuestions.Queries;
using OPS.Application.Features.McqOptions.Commands;
using OPS.Application.Features.McqOptions.Queries;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;


public class McqOptionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllMcqOption()
    {
        var mcqOption = await _mediator.Send(new GetAllMcqOptionQuery());

        return ToResult(mcqOption);
    }

    [HttpGet("{mcqOptionId:guid}")]
    public async Task<IActionResult> GetMcqOptionByIdAsync(Guid mcqOptionId)
    {
        var result = await _mediator.Send(new GetMcqOptionByIdQuery(mcqOptionId));

        return ToResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateMcqOptionCommand command)
    {
        var createdMcqOption = await _mediator.Send(command);

        return ToResult(createdMcqOption);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAccount(UpdateMcqOptionCommand command)
    {
        var mcqOption = await _mediator.Send(command);

        return ToResult(mcqOption);
    }
}