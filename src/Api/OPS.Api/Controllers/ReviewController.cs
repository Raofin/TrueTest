using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Review.Queries;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/Review")]
public class ReviewController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Retrieves an exam with questions and submissions of a candidate.</summary>
    /// <param name="examId">Exam Id.</param>
    /// <param name="accountId">Account Id of a candidate.</param>
    /// <returns>Exam with questions and submissions.</returns>
    [HttpGet("Exam/{examId:guid}/{accountId:guid}")]
    [EndpointDescription("Retrieves an exam with questions and submissions of a candidate.")]
    [ProducesResponseType<OngoingExamResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> GetOngoingExamAsync(Guid examId, Guid accountId)
    {
        var query = new GetExamByCandidateQuery(examId, accountId);
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}