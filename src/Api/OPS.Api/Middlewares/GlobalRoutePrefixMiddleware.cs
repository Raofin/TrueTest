namespace OPS.Api.Middlewares;

/// <summary>
/// Middleware that prepends a global route prefix to the request path.
/// </summary>
public class GlobalRoutePrefixMiddleware(RequestDelegate next, string routePrefix)
{
    private readonly RequestDelegate _next = next;
    private readonly string _routePrefix = routePrefix;

    /// <summary>
    /// Invokes the middleware to process the HTTP request.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.PathBase = new PathString(_routePrefix);
        await _next(context);
    }
}