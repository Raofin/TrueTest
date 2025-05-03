using OPS.Application.Dtos;
using OPS.Domain.Enums;

namespace OPS.Application.Interfaces;

/// <summary>
/// Defines the contract for interacting with a "One Compiler" service to run code snippets.
/// </summary>
public interface IOneCompilerService
{
    /// <summary>
    /// Asynchronously executes a code snippet using the specified language and optional input.
    /// </summary>
    /// <param name="languageId">The <see cref="LanguageId"/> of the programming language for the code.</param>
    /// <param name="code">The code snippet to be executed.</param>
    /// <param name="input">Optional input to be provided to the code during execution.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation. The task result contains
    /// a <see cref="CodeRunResponse"/> object with the output and status of the code execution.
    /// </returns>
    Task<CodeRunResponse> CodeRunAsync(LanguageId languageId, string code, string? input);
}