using Refit;

namespace OPS.Infrastructure.OneCompiler.Refit;

/// <summary>
/// Defines the Refit interface for interacting with the OneCompiler API.
/// </summary>
internal interface IOneCompilerClient
{
    /// <summary>
    /// Sends a request to the OneCompiler API's run endpoint to execute code.
    /// </summary>
    /// <param name="runRequest">The <see cref="CodeRunRequest"/> object containing the language, code, and optional input.</param>
    /// <returns>A <see><cref>Task{string}</cref></see> representing the asynchronous operation. The task result contains the JSON response from the OneCompiler API.</returns>
    [Post("/api/v1/run")]
    Task<string> CodeRunAsync([Body] CodeRunRequest runRequest);
}