using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.AiPrompts.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("Ai")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class AiPromptController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Prompt")]
    [ProducesResponseType<string>(Status200OK)]
    public async Task<IActionResult> GetPrompt(AiPromptCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    [HttpPost("Create/ExamDescription")]
    [ProducesResponseType<AiExamDescriptionResponse>(Status200OK)]
    public async Task<IActionResult> CreateExamDescription(AiExamDescriptionQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    [HttpPost("Review/ProblemSubmission/{problemSubmissionId}")]
    [ProducesResponseType<AiSubmissionReview>(Status200OK)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewProblemSubmission(Guid problemSubmissionId)
    {
        var response = await _mediator.Send(new AiReviewProblemQuery(problemSubmissionId));
        return ToResult(response);
    }

    [HttpPost("Review/ExamSubmission/{examSubmissionId}")]
    [ProducesResponseType<AiSubmissionReview>(Status200OK)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewExamSubmission(Guid writtenSubmissionId)
    {
        var response = await _mediator.Send(new AiReviewWrittenQuery(writtenSubmissionId));
        return ToResult(response);
    }

    [HttpPost("Create/ProblemSolvingQuestion")]
    [ProducesResponseType<AiProblemQuestionResponse>(Status200OK)]
    public async Task<IActionResult> CreateProblemSolvingQuestion(AiCreateProblemQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    [HttpPost("Create/WrittenQuestion")]
    [ProducesResponseType<AiWrittenQues>(Status200OK)]
    public async Task<IActionResult> CreateWrittenQuestion(AiCreateWrittenQuesQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}