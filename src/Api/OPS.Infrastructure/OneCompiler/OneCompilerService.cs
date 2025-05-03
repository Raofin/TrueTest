using Newtonsoft.Json;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;
using OPS.Domain.Enums;
using OPS.Infrastructure.OneCompiler.Refit;
using Throw;

namespace OPS.Infrastructure.OneCompiler;

/// <summary>
/// Implementation of the <see cref="IOneCompilerService"/> interface for interacting with the OneCompiler API.
/// </summary>
internal class OneCompilerService(IOneCompilerClient oneCompilerClient) : IOneCompilerService
{
    private readonly IOneCompilerClient _oneCompiler = oneCompilerClient;

    /// <inheritdoc />
    public async Task<CodeRunResponse> CodeRunAsync(LanguageId languageId, string code, string? input)
    {
        var fileName = LanguageHelper.GetDefaultFileName(languageId);

        var request = new CodeRunRequest(
            languageId.ToString(),
            [new CodeFile(fileName, code)],
            input
        );

        var responseStr = await _oneCompiler.CodeRunAsync(request);
        var response = JsonConvert.DeserializeObject<CodeRunResponse>(responseStr);
        response.ThrowIfNull("Invalid response from OneCompiler API");
        return response;
    }
}