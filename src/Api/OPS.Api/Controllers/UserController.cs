using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.User.Commands;
using OPS.Application.Features.User.Queries;
using OPS.Domain.Contracts.Core.Authentication;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("api/User")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
public class UserController(IMediator mediator, IUserInfoProvider userInfoProvider) : BaseApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    /// <summary>Gets authenticated user info.</summary>
    /// <returns>Authenticated user details.</returns>
    [HttpGet("Info")]
    [EndpointDescription("")]
    [ProducesResponseType(Status200OK)]
    public IActionResult GetInfo()
    {
        return !_userInfoProvider.IsAuthenticated()
            ? Unauthorized("User is not authenticated.")
            : Ok(new
            {
                AccountId = _userInfoProvider.AccountId(),
                Username = _userInfoProvider.Username(),
                Email = _userInfoProvider.Email(),
                Roles = _userInfoProvider.Roles()
            });
    }

    /// <summary>Updates account settings of the authenticated user.</summary>
    /// <param name="command">Account setting details to update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated account settings.</returns>
    [HttpPatch("AccountSettings")]
    [EndpointDescription("Updates account settings of the authenticated user.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAccountSettingsAsync(UpdateAccountSettingsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Retrieves account details of the authenticated user.</summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>User details.</returns>
    [HttpGet("Details")]
    [EndpointDescription("Retrieves account details of the authenticated user.")]
    [ProducesResponseType<AccountWithDetailsResponse>(Status200OK)]
    public async Task<IActionResult> GetDetailsAsync(CancellationToken cancellationToken)
    {
        var query = new GetUserDetailsQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Creates or updates the authenticated user profile.</summary>
    /// <param name="command">Profile details.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Updated or created profile.</returns>
    [HttpPost("SaveProfile")]
    [EndpointDescription("Creates or updates the authenticated user profile.")]
    [ProducesResponseType<ProfileResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateOrUpdateProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Deletes a profile link of the authenticated user.</summary>
    /// <param name="command">Profile link Id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns></returns>
    [HttpDelete("ProfileLink")]
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