using Refit;

namespace OPS.Infrastructure.OneCompiler.Refit;

internal interface IOneCompilerApi
{
    [Post("/api/v1/run")]
    Task<string> CodeRunAsync([Body] CodeRunRequest runRequest);
}