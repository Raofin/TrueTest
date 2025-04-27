using OPS.Application.Dtos;
using OPS.Domain.Enums;

namespace OPS.Application.Interfaces;

public interface IOneCompilerService
{
    Task<CodeRunResponse> CodeRunAsync(LanguageId languageId, string code, string? input);
}