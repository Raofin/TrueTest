using Refit;

namespace OPS.Infrastructure.OneCompiler.Refit;

internal interface IOneCompilerClient
{
    [Post("/api/v1/run")]
    Task<string> CodeRunAsync([Body] CodeRunRequest runRequest);
}