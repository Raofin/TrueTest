using OPS.Domain.Enums;

namespace OPS.Domain.Contracts.Core.OneCompiler;

public interface IOneCompilerApiService
{
    Task<CodeRunResponse> CodeRunAsync(LanguageId languageId, string code, string? input);
}