using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Candidates.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

public class CandidateController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves all exams of the authenticated user.</summary>
    /// <returns>List of exams by user.</returns>
    [HttpGet("Exams")]
    [EndpointDescription("Retrieves all exams of the authenticated user.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetExamsAsync()
    {
        var query = new GetAllExamsByCandidateQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}