using OPS.Application.Dtos;

namespace OPS.Application.Interfaces;

/// <summary>
/// Defines the contract for interacting with an AI service to generate responses based on prompts.
/// </summary>
public interface IAiService
{
    /// <summary>
    /// Sends a prompt to the AI service and asynchronously retrieves the response, deserialized to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the AI service's response should be deserialized.</typeparam>
    /// <param name="prompt">The <see cref="PromptRequest"/> containing the input for the AI service.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation. The task result contains
    /// an instance of type <typeparamref name="T"/> if the prompt was successful and the response
    /// could be deserialized; otherwise, it returns <c>null</c>.
    /// </returns>
    Task<T?> PromptAsync<T>(PromptRequest prompt);
}