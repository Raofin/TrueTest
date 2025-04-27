using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.User.Commands;
using OPS.Application.Features.User.Queries;
using OPS.Application.Interfaces.Auth;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("User")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class UserController(IMediator mediator, IUserProvider userProvider) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserProvider _userProvider = userProvider;

    /// <summary>Gets decoded token values.</summary>
    /// <returns>Authenticated user details.</returns>
    [Authorize]
    [HttpGet("DecodeToken")]
    [EndpointDescription("Gets decoded token values.")]
    [ProducesResponseType(Status200OK)]
    public IActionResult DecodeToken()
    {
        return Ok(_userProvider.DecodeToken());
    }

    /// <summary>Retrieves account details of the authenticated user.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>User details.</returns>
    [HttpGet("Details")]
    [Authorize]
    [EndpointDescription("Retrieves account details of the authenticated user.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    public async Task<IActionResult> GetDetailsAsync(CancellationToken cancellationToken)
    {
        var query = new GetUserDetailsQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates account settings of the authenticated user.</summary>
    /// <param name="command">Account setting details to update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated account settings.</returns>
    [HttpPatch("Account/Settings")]
    [Authorize]
    [EndpointDescription("Updates account settings of the authenticated user.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAccountSettingsAsync(UpdateAccountSettingsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates or updates the authenticated user profile.</summary>
    /// <param name="command">Profile details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated or created profile.</returns>
    [HttpPut("Profile/Save")]
    [Authorize]
    [EndpointDescription("Creates or updates the authenticated user profile.")]
    [ProducesResponseType<ProfileResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(SaveProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a profile link of the authenticated user.</summary>
    /// <param name="command">Profile link Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns></returns>
    [HttpDelete("Profile/Link")]
    [Authorize]
    [EndpointDescription("Deletes a profile link of the authenticated user.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteProfileLinkAsync(DeleteProfileLinkCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }
}