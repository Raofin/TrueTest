using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Features.ExamQuestions.Commands;
using OPS.Application.Features.ExamQuestions.Queries;

namespace OPS.Api.Controllers;

public class QuestionController(
    IMediator mediator,
    IValidator<CreateQuestionCommand> createQuestionValidator,
    IValidator<UpdateQuestionCommand> updateQuestionValidator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<CreateQuestionCommand> _createQuestionValidator = createQuestionValidator;
    private readonly IValidator<UpdateQuestionCommand> _updateQuestionValidator = updateQuestionValidator;

    [HttpGet]
    public async Task<IActionResult> GetAllQuestionsAsync()
    {
        var query = new GetAllQuestionsQuery();

        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }

    [HttpGet("question/{questionId:guid}")]
    public async Task<IActionResult> GetQuestionByIdAsync(Guid questionId)
    {
        var query = new GetQuestionByIdQuery(questionId);

        var result = await _mediator.Send(query);

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => Problem()
            };
    }

    [HttpGet("exam/{examId:guid}")]
    public async Task<IActionResult> GetAllQuestionsByExamIdAsync(Guid examId)
    {
        var query = new GetAllQuestionByExamIdQuery(examId);

        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateQuestionCommand command)
    {
        var validation = await _createQuestionValidator.ValidateAsync(command);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateQuestionCommand command)
    {
        var validation = await _updateQuestionValidator.ValidateAsync(command);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return BadRequest(new { errors });
        }

        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid QuestionId)
    {
        var command = new DeleteQuestionCommand(QuestionId);

        var result = await _mediator.Send(command);

        return !result.IsError
            ? Ok()
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound(),
                _ => Problem()
            };
    }
}