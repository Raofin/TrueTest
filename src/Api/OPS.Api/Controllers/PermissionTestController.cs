using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common.ErrorResponses;
using OPS.Domain.Constants;
using OPS.Infrastructure.Auth.Permission;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

/// <summary>
/// [Obsolete] - Controller for testing permission attributes.
/// </summary>
[Obsolete("This controller is obsolete and used for testing permission attributes.")]
[Route("PermissionTest")]
[ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
[ProducesResponseType<ForbiddenResponse>(Status403Forbidden)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class PermissionTestController : ControllerBase
{
    /// <summary>[Obsolete] - Endpoint requiring the 'SubmitAnswers' permission.</summary>
    /// <returns>OK if the user has the 'SubmitAnswers' permission.</returns>
    [HttpGet("Candidate")]
    [HasPermission(Permissions.SubmitAnswers)]
    [EndpointDescription("[Obsolete] - Endpoint requiring the 'SubmitAnswers' permission.")]
    public IActionResult Test1()
    {
        return Ok();
    }

    /// <summary>[Obsolete] - Endpoint requiring the 'ManageAccounts' permission.</summary>
    /// <returns>OK if the user has the 'ManageAccounts' permission.</returns>
    [HttpGet("Admin")]
    [HasPermission(Permissions.ManageAccounts)]
    [EndpointDescription("[Obsolete] - Endpoint requiring the 'ManageAccounts' permission.")]
    public IActionResult Test2()
    {
        return Ok();
    }

    /// <summary>[Obsolete] - Endpoint requiring the 'ReviewSubmission' permission.</summary>
    /// <returns>OK if the user has the 'ReviewSubmission' permission.</returns>
    [HttpGet("Moderator")]
    [HasPermission(Permissions.ReviewSubmission)]
    [EndpointDescription("[Obsolete] - Endpoint requiring the 'ReviewSubmission' permission.")]
    public IActionResult Test3()
    {
        return Ok();
    }

    /// <summary>[Obsolete] - Endpoint that allows anonymous access.</summary>
    /// <returns>OK for any unauthenticated or authenticated user.</returns>
    [HttpGet("Anonymous")]
    [AllowAnonymous]
    [EndpointDescription("[Obsolete] - Endpoint that allows anonymous access.")]
    public IActionResult Test4()
    {
        return Ok();
    }
}