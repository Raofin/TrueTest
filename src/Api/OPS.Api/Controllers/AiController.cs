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
    [ProducesResponseType(Status200OK)]
    public async Task<IActionResult> GetPrompt(AiPromptCommand command)
    {
        var response = await _mediator.Send(command);

        return !response.IsError
            ? Ok(new { response = response.Value })
            : ToResult(response);
    }

    [HttpPost("Generate/ExamDescription")]
    [ProducesResponseType<AiExamDescriptionResponse>(Status200OK)]
    public async Task<IActionResult> GenerateExamDescription(AiExamDescriptionQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    [HttpPost("Generate/ProblemSolvingQuestion")]
    [ProducesResponseType<AiProblemQuestionResponse>(Status200OK)]
    public async Task<IActionResult> GenerateProblemSolvingQuestion(AiGenerateProblemQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    [HttpPost("Generate/WrittenQuestion")]
    [ProducesResponseType<AiWrittenQues>(Status200OK)]
    public async Task<IActionResult> GenerateWrittenQuestion(AiGenerateWrittenQuesQuery query)
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

    [HttpPost("Review/WrittenSubmission/{examSubmissionId}")]
    [ProducesResponseType<AiSubmissionReview>(Status200OK)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> ReviewWrittenSubmission(AiReviewWrittenQuery query)
    {
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}