using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Infrastructure.Authentication.Permission;

namespace OPS.Api.Controllers;

[Obsolete]
[Route("PermissionTest")]
public class PermissionTestController : ControllerBase
{
    [HttpGet("Candidate")]
    [HasPermission(Permissions.SubmitAnswers)]
    public IActionResult Test1()
    {
        return Ok();
    }

    [HttpGet("Admin")]
    [HasPermission(Permissions.ManageAccounts)]
    public IActionResult Test2()
    {
        return Ok();
    }

    [HttpGet("Moderator")]
    [HasPermission(Permissions.ReviewSubmission)]
    public IActionResult Test3()
    {
        return Ok();
    }

    [HttpGet("Anonymous")]
    [AllowAnonymous]
    public IActionResult Test4()
    {
        return Ok();
    }
}