using Refit;

namespace OPS.Infrastructure.Gemini.Refit;

/// <summary>
/// Defines the Refit interface for interacting with the Gemini API.
/// </summary>
public interface IGeminiClient
{
    /// <summary>
    /// Sends a request to the Gemini API's generateContent endpoint.
    /// </summary>
    /// <param name="jsonPayload">The JSON payload containing the prompt and other request parameters.</param>
    /// <param name="model">The specific Gemini model to use for content generation.</param>
    /// <param name="key">The API key for authenticating with the Gemini API.</param>
    /// <returns>A <see><cref>Task{string}</cref></see> representing the asynchronous operation. The task result contains the JSON response from the Gemini API.</returns>
    [Post("/{model}:generateContent")]
    Task<string> SendRequestAsync(
        [Body] string jsonPayload,
        [AliasAs("model")] string model,
        [Query] string key
    );
}