using Newtonsoft.Json;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;
using OPS.Domain.Enums;
using OPS.Infrastructure.OneCompiler.Refit;
using Throw;

namespace OPS.Infrastructure.OneCompiler;

internal class OneCompilerService(IOneCompilerApi api) : IOneCompilerService
{
    private readonly IOneCompilerApi _api = api;

    public async Task<CodeRunResponse> CodeRunAsync(LanguageId languageId, string code, string? input)
    {
        var fileName = LanguageHelper.GetDefaultFileName(languageId);

        var request = new CodeRunRequest(
            languageId.ToString(),
            [new CodeFile(fileName, code)],
            input
        );

        var responseStr = await _api.CodeRunAsync(request);
        var response = JsonConvert.DeserializeObject<CodeRunResponse>(responseStr);
        response.ThrowIfNull("Invalid response from OneCompiler API");
        return response;
    }
}