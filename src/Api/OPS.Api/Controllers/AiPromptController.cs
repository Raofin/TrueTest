using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Features.AiPrompts.Command;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("Prompt")]
[Produces("application/json")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class AiPromptController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType<string>(Status200OK)]
    public async Task<IActionResult> GetPrompt(PromptCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }
}