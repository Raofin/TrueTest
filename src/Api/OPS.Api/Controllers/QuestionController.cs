using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.ExamQuestions.Commands;
using OPS.Application.Features.ExamQuestions.Queries;

namespace OPS.Api.Controllers;

public class QuestionController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllQuestionsAsync()
    {
        var result = await _mediator.Send(new GetAllQuestionsQuery());

        return ToResult(result);    
    }

    [HttpGet("question/{questionId:guid}")]
    public async Task<IActionResult> GetQuestionByIdAsync(Guid questionId)
    {
        var result = await _mediator.Send(new GetQuestionByIdQuery(questionId));

        return ToResult(result);
    }

    [HttpGet("exam/{examId:guid}")]
    public async Task<IActionResult> GetAllQuestionsByExamIdAsync(Guid examId)
    {
        var result = await _mediator.Send(new GetAllQuestionByExamIdQuery(examId));
        return ToResult(result);
    }

    [HttpGet("{examId:guid}/{questionTypeId:int}")]
    public async Task<IActionResult> GetAllQuestionsByExamIdQuestionTypeIdAsync(Guid examId,int questionTypeId)
    {
        var result = await _mediator.Send(new GetAllQuestionByExamIdQuestionTypeIdQuery(examId,questionTypeId));
        return ToResult(result);
    }


    [HttpGet("scoreByExamId/{examId:guid}")]
    public async Task<IActionResult> GetScoreByExamIdAsync(Guid examId)
    {
        var result = await _mediator.Send(new GetScoreByExamsIdQuery(examId));

        return ToResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateQuestionCommand command)
    {
        var result = await _mediator.Send(command);
        return ToResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateQuestionCommand command)
    {

        var result = await _mediator.Send(command);
        
        return ToResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid QuestionId)
    {
        var result = await _mediator.Send(new DeleteQuestionCommand(QuestionId));
        
        return ToResult(result);
    }
}