using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OPS.Api.Common;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult ToResult<T>(ErrorOr<T> result)
    {
        if (result.IsError)
            return Problem(result.Errors);

        return result.Value is Unit or Success
            ? Ok()
            : Ok(result.Value);
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0) return Problem();

        return errors.All(error => error.Type == ErrorType.Validation)
            ? ValidationProblem(errors)
            : Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, detail: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var state = new ModelStateDictionary();

        foreach (var error in errors)
            state.AddModelError(error.Code, error.Description);

        return ValidationProblem(
            type: "ValidationError",
            modelStateDictionary: state);
    }
}