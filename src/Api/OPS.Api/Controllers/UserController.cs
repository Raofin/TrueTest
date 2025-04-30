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

/// <summary>
/// API endpoints for managing the authenticated user's account and profile.
/// </summary>
[Route("User")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class UserController(IMediator mediator, IUserProvider userProvider) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserProvider _userProvider = userProvider;

    /// <summary>Gets the values decoded from the user's authentication token.</summary>
    /// <returns>Authenticated user details extracted from the token.</returns>
    [Authorize]
    [HttpGet("DecodeToken")]
    [EndpointDescription("Gets the values decoded from the user's authentication token.")]
    [ProducesResponseType(Status200OK)]
    public IActionResult DecodeToken()
    {
        return Ok(_userProvider.DecodeToken());
    }

    /// <summary>Retrieves detailed account information for the currently authenticated user.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Detailed account information of the authenticated user.</returns>
    [HttpGet("Details")]
    [Authorize]
    [EndpointDescription("Retrieves detailed account information for the currently authenticated user.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    public async Task<IActionResult> GetDetailsAsync(CancellationToken cancellationToken)
    {
        var query = new GetUserDetailsQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Updates the settings of the currently authenticated user's account.</summary>
    /// <param name="command">Details of the account settings to be updated.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated account information after applying the new settings.</returns>
    [HttpPatch("Account/Settings")]
    [Authorize]
    [EndpointDescription("Updates the settings of the currently authenticated user's account.")]
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

    /// <summary>Creates or updates the profile of the currently authenticated user.</summary>
    /// <param name="command">Details of the user profile to be saved or updated.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The updated or newly created user profile information.</returns>
    [HttpPut("Profile/Save")]
    [Authorize]
    [EndpointDescription("Creates or updates the profile of the currently authenticated user.")]
    [ProducesResponseType<ProfileResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(SaveProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a specific profile link associated with the authenticated user's profile.</summary>
    /// <param name="command">Identifier of the profile link to be deleted.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>No content upon successful deletion of the profile link.</returns>
    [HttpDelete("Profile/Link")]
    [Authorize]
    [EndpointDescription("Deletes a specific profile link associated with the authenticated user's profile.")]
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