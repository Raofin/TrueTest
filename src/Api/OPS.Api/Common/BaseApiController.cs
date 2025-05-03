using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Net.Mime.MediaTypeNames.Application;

namespace OPS.Api.Common;

/// <summary>
/// Base API controller providing standardized response handling for ErrorOr results.
/// </summary>
[Authorize]
[ApiController]
[Produces(Json)]
[Consumes(Json)]
public class BaseApiController : ControllerBase
{
    /// <summary>
    /// Converts an ErrorOr result to an IActionResult.
    /// Returns an Ok result with the value for success, or a Problem result for errors.
    /// </summary>
    /// <typeparam name="T">The type of the value in the ErrorOr result.</typeparam>
    /// <param name="result">The ErrorOr result to convert.</param>
    /// <returns>An IActionResult representing the outcome of the ErrorOr result.</returns>
    protected IActionResult ToResult<T>(ErrorOr<T> result)
    {
        if (result.IsError)
            return Problem(result.Errors);

        return result.Value is Unit or Success
            ? Ok()
            : Ok(result.Value);
    }

    /// <summary>
    /// Creates an IActionResult Problem based on a list of errors.
    /// If all errors are validation errors, a ValidationProblem is returned.
    /// Otherwise, the first error is used to create a Problem result.
    /// </summary>
    /// <param name="errors">A list of Error objects.</param>
    /// <returns>An IActionResult representing the errors.</returns>
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0) return Problem();

        return errors.All(error => error.Type == ErrorType.Validation)
            ? ValidationProblem(errors)
            : Problem(errors[0]);
    }

    /// <summary>
    /// Creates an IActionResult Problem based on a single error.
    /// The status code of the Problem result is determined by the ErrorType.
    /// </summary>
    /// <param name="error">The Error object.</param>
    /// <returns>An IActionResult representing the error with an appropriate status code.</returns>
    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, detail: error.Description);
    }

    /// <summary>
    /// Creates an IActionResult ValidationProblem based on a list of validation errors.
    /// Adds each error to the ModelStateDictionary.
    /// </summary>
    /// <param name="errors">A list of Error objects with ErrorType.Validation.</param>
    /// <returns>An IActionResult ValidationProblem containing the validation errors.</returns>
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