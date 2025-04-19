using OPS.Domain.Contracts.Core.OneCompiler;
using Newtonsoft.Json;
using OPS.Domain.Enums;
using OPS.Infrastructure.OneCompiler.Refit;
using Throw;

namespace OPS.Infrastructure.OneCompiler;

internal class OneCompilerApiService(IOneCompilerApi api) : IOneCompilerApiService
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
        CodeRunResponse? response = JsonConvert.DeserializeObject<CodeRunResponse>(responseStr);
        response.ThrowIfNull("Invalid response from OneCompiler API");
        return response;
    }
}