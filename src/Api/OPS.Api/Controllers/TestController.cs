using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Infrastructure.Authentication.Permission;

namespace OPS.Api.Controllers;

public class TestController : ControllerBase
{
    [HttpGet("Permission/Candidate")]
    [HasPermission(Permissions.SubmitAnswers)]
    public IActionResult Test1()
    {
        return Ok();
    }
    
    [HttpGet("Permission/Admin")]
    [HasPermission(Permissions.ManageAccounts)]
    public IActionResult Test2()
    {
        return Ok();
    }
    
    [HttpGet("Permission/Moderator")]
    [HasPermission(Permissions.ReviewSubmission)]
    public IActionResult Test3()
    {
        return Ok();
    }
    
    [HttpGet("Permission/Anonymous")]
    [AllowAnonymous]
    public IActionResult Test4()
    {
        return Ok();
    }
}