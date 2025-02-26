
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Application.CrossCutting.Attributes;
using OPS.Application.Features.Profiles.Commands;
using OPS.Application.Features.Profiles.Queries;
using OPS.Domain.Enums;

namespace OPS.Api.Controllers;
public class ProfileController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{profileId:guid}")]
    public async Task<IActionResult> GetExamByIdAsync(Guid profileId)
    {
        var result = await _mediator.Send(new GetProfileByIdQuery(profileId));

        return ToResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProfileCommand command)
    {
        
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command);

        return ToResult(result);
    }
}