namespace OPS.Api.Middlewares;

/// <summary>
/// Middleware that handles exceptions occurring during the processing of an HTTP request.
/// </summary>
public class ExceptionHandleMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Invokes the middleware to process the HTTP request.
    /// </summary>
    /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                new { message = "An unexpected error occurred. Please try again later." }
            );
        }
    }
}