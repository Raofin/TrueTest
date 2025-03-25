using MediatR;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Features.Examinations.Queries;
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
    /// <returns>Updated account settings.</returns>
    [HttpPatch("AccountSettings")]
    [EndpointDescription("Updates account settings of the authenticated user.")]
    [ProducesResponseType<AccountResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<ConflictResponse>(Status409Conflict)]
    public async Task<IActionResult> UpdateAccountSettingsAsync(UpdateAccountSettingsCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves account details of the authenticated user.</summary>
    /// <returns>User details.</returns>
    [HttpGet("Details")]
    [EndpointDescription("Retrieves account details of the authenticated user.")]
    [ProducesResponseType<AccountResponse>(Status200OK)]
    public async Task<IActionResult> GetDetailsAsync()
    {
        var query = new GetUserDetailsQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }

    /// <summary>Creates or updates the authenticated user profile.</summary>
    /// <param name="command">Profile details.</param>
    /// <returns>Updated or created profile.</returns>
    [HttpPost("SaveProfile")]
    [EndpointDescription("Creates or updates the authenticated user profile.")]
    [ProducesResponseType<ProfileResponse>(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateOrUpdateProfileCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Deletes a profile link of the authenticated user.</summary>
    /// <param name="command">Profile link Id.</param>
    /// <returns>Void.</returns>
    [HttpDelete("ProfileLink")]
    [EndpointDescription("Deletes a profile link of the authenticated user.")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteProfileLinkAsync(DeleteProfileLinkCommand command)
    {
        var response = await _mediator.Send(command);
        return ToResult(response);
    }

    /// <summary>Retrieves exams of the authenticated user.</summary>
    /// <returns>List of exams by user.</returns>
    [HttpGet("Exams")]
    [EndpointDescription("Retrieves exams of the authenticated user.")]
    [ProducesResponseType<List<ExamResponse>>(Status200OK)]
    public async Task<IActionResult> GetExamsAsync()
    {
        var query = new GetExamsByUserQuery();
        var response = await _mediator.Send(query);
        return ToResult(response);
    }
}